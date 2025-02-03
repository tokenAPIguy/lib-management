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
                CategoryID = obj.Category.ID,
                SubCategoryIDs = string.Join(",", obj.SubCategories.Select(s => s.ID)),
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

    [HttpPut]
    public async Task<IResult> Put(BookDTO obj) {
        try {
            List<Book>? books = await db.Books
                .Where(b => b.Name == obj.Name)
                .ToListAsync();

            if (!books.Any()) {
                return Results.NotFound();
            }

            List<int> foo = (obj.SubCategories.Select(x => x.ID)).ToList();
            string bar = string.Join(",", foo);

            foreach (Book book in books) {
                book.CategoryID = obj.Category.ID;
                book.SubCategoryIDs = bar;
            }
            
            return Results.Ok();
        }
        catch (Exception e) {
            return Results.Problem(e.Message, statusCode: 500);            
        }
    }
    
    
    [HttpDelete]
    public async Task<IResult> Delete(int? id) {
        try {
            Book? book = await db.Books.FindAsync(id);

            if (book == null) {
                return Results.NotFound();    
            }
            
            db.Remove(book);
            await db.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception e) {
            return Results.Problem(e.Message, statusCode: 500);            
        }
    }
}