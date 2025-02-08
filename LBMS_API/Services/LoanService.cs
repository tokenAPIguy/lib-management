using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Models;
using Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Services;

public class LoanService(HttpContext httpContext, ApplicationDbContext db) {
    private readonly Logger logger = Logger.Default;

    [HttpGet]
    public async Task<IResult> Get(Guid? id) {
        PathString path = httpContext.Request.Path;
        IQueryable<Loan> query = db.Loans.AsQueryable();

        if (id != null) {
            query = query.Where(l => l.ID == id);
            Loan? loan = await query.FirstOrDefaultAsync();
            if (loan != null) {
                logger.Information($"HTTP/1.1 GET - 200 {path}/{loan.ID}");
                return Results.Ok(loan);
            }
            logger.Information($"HTTP/1.1 GET - 404 {path}/{loan.ID}");
            return Results.NotFound();
        }
    
        List<Loan> loans = await query.ToListAsync();
        if (loans.Any()) {
            logger.Information($"HTTP/1.1 GET - 200 {path}");
            return Results.Ok(loans);
        }
        logger.Information($"HTTP/1.1 GET - 404 {path}");
        return Results.NotFound();
    }

    [HttpPut]
    public async Task<IResult> Put(Guid? id, LoanDTO obj) {
        PathString path = httpContext.Request.Path;
        try {
            Loan? loan = await db.Loans.FindAsync(id);
            if (loan == null) {
                logger.Information($"HTTP/1.1 PUT - 404 {path}/{id}");
                return Results.NotFound();
            }
            
            loan.ReturnedDate = DateOnly.FromDateTime(DateTime.Now);
            loan.Status = LoanStatus.Returned;
            
            db.Update(loan);
            await db.SaveChangesAsync();
            
            logger.Information($"HTTP/1.1 PUT - 201 {path}/{id}");
            return Results.Ok();

        }
        catch (Exception e) {
            logger.Information($"HTTP/1.1 POST - 500 {path} - {e.Message}");
            logger.Error($"{e.Message}\n{e.StackTrace}", "error.log"); 
            return Results.Problem(e.Message, statusCode: 500);
        }    
    }
    
    [HttpPost]
    public async Task<IResult> Post(LoanDTO obj) {
        PathString path = httpContext.Request.Path;
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
            logger.Information($"HTTP/1.1 POST - 201 {path}/{loan.ID}");
            return Results.Created();
        } 
        catch (Exception e) {
            logger.Information($"HTTP/1.1 POST - 500 {path} - {e.Message}");
            logger.Error($"{e.Message}\n{e.StackTrace}", "error.log"); 
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
}