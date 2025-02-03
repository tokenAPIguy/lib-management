using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Services;

public class LoanService(ApplicationDbContext db) {
    [HttpGet]
    public async Task<IResult> Get(Guid? id) {
        IQueryable<Loan> query = db.Loans.AsQueryable();

        if (id != null) {
            query = query.Where(l => l.ID == id);
            Loan? loan = await query.FirstOrDefaultAsync();
            return loan != null
                ? Results.Ok(loan)
                : Results.NotFound();
        }
    
        List<Loan> loans = await query.ToListAsync();
        return loans.Any()
            ? Results.Ok(loans)
            : Results.NotFound();
    }

    [HttpPut]
    public async Task<IResult> Put(Guid? id, LoanDTO obj) {
        try {
            Loan? loan = await db.Loans.FindAsync(id);

            if (loan == null) {
                return Results.NotFound();
            }
            
            loan.ReturnedDate = DateOnly.FromDateTime(DateTime.Now);
            loan.Status = LoanStatus.Returned;
            
            db.Update(loan);
            await db.SaveChangesAsync();
            return Results.Ok();

        }
        catch (Exception e) {
            return Results.Problem(e.Message, statusCode: 500);
        }    
    }
    
    [HttpPost]
    public async Task<IResult> Post(LoanDTO obj) {
        try {
            Loan loan = new() {
                ID = Guid.NewGuid(),
                BookID = obj.BookID,
                UserID = obj.UserID,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now),
                DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(14)),
                Status = LoanStatus.Active,
            };
            
            db.Loans.Add(loan);
            await db.SaveChangesAsync();
            return Results.Created();
        } 
        catch (Exception e) {
            // log e.stacktrace
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
}