using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Services;

public class BookService(ApplicationDbContext db) {
    [HttpGet]
    public async Task<IResult> Get(int? id) {
        IQueryable<Book> query = db.Books.AsQueryable();
    
        if (id != null) {
            query = query.Where(b => b.ID == id);
            Book? book = await query.FirstOrDefaultAsync();
            return book != null 
                ? Results.Ok(book) 
                : Results.NotFound();
        }
    
        List<Book> books = await query.ToListAsync();
        return books.Any() 
            ? Results.Ok(books) 
            : Results.NotFound();
    }

    [HttpPost]
    public async Task<IResult> Post(BookDTO obj) {
        try {
            Book book = new() {
                Name = obj.Name,
                Author = obj.Author,
                ISBN = obj.ISBN,
                Description = obj.Description,
                Category = obj.Category,
                SubCategories = obj.SubCategories,
                IsAvailable = true
            };
            
            db.Add(book);
            await db.SaveChangesAsync();
            return Results.Ok(book);
        }
        catch (Exception e) {
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
}