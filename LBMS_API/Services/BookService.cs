using System.Runtime.InteropServices.JavaScript;
using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Models;
using Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Services;

public class BookService(HttpContext httpContext, ApplicationDbContext db) {
    private readonly Logger logger = Logger.Default;

    [HttpGet]
    public async Task<IResult> Get(int? id) {
        PathString path = httpContext.Request.Path;
        IQueryable<Book> query = db.Books.AsQueryable();
    
        if (id != null) {
            query = query.Where(b => b.ID == id);
            Book? book = await query.FirstOrDefaultAsync();
            
            if (book != null) {
                logger.Information($"HTTP/1.1 GET - 200 {path}/{book.ID}");
                return Results.Ok(book);
            } 
            logger.Information($"HTTP/1.1 GET - 404 {path}/{book.ID}");
            return Results.NotFound();
        }
    
        List<Book> books = await query.ToListAsync();
        if (books.Any()) {
            logger.Information($"HTTP/1.1 GET - 200 {path}");
            Results.Ok(books);
        } 
        logger.Information($"HTTP/1.1 GET - 404 {path}");
        return Results.NotFound();
    }

    [HttpPost]
    public async Task<IResult> Post(BookDTO obj) {
        PathString path = httpContext.Request.Path;
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
        
            logger.Information($"HTTP/1.1 POST - 201 {path}/{book.ID}");
            return Results.Created();
        }
        catch (Exception e) {
            logger.Information($"HTTP/1.1 POST - 500 {path} - {e.Message}");
            logger.Error($"{e.Message}\n{e.StackTrace}", "error.log");
            return Results.Problem(e.Message, statusCode: 500);
        }
    }

    [HttpPut]
    public async Task<IResult> Put(BookDTO obj) {
        PathString path = httpContext.Request.Path;
        try {
            List<Book>? books = await db.Books
                .Where(b => b.Name == obj.Name)
                .ToListAsync();

            if (!books.Any()) {
                logger.Information($"HTTP/1.1 PUT - 404 {path}/{obj.Name}");
                return Results.NotFound();
            }

            List<int> foo = (obj.SubCategories.Select(x => x.ID)).ToList();
            string bar = string.Join(",", foo);

            foreach (Book book in books) {
                book.CategoryID = obj.Category.ID;
                book.SubCategoryIDs = bar;
            }
            logger.Information($"HTTP/1.1 PUT - 200 {path} | {obj.Name}");
            return Results.Ok();
        }
        catch (Exception e) {
            logger.Information($"HTTP/1.1 PUT - 500 {path} | {obj.Name} - {e.Message}");
            logger.Error($"{e.Message}\n{e.StackTrace}", "error.log");
            return Results.Problem(e.Message, statusCode: 500);            
        }
    }
    
    
    [HttpDelete]
    public async Task<IResult> Delete(int? id) {
        PathString path = httpContext.Request.Path;
        try {
            Book? book = await db.Books.FindAsync(id);

            if (book == null) {
                logger.Information($"HTTP/1.1 DELETE - 404 {path}/{id}");
                return Results.NotFound();    
            }
            
            db.Remove(book);
            await db.SaveChangesAsync();
            logger.Information($"HTTP/1.1 DELETE - 200 {path}/{book.ID}");
            return Results.NoContent();
        }
        catch (Exception e) {
            logger.Information($"HTTP/1.1 POST - 500 {path} - {e.Message}");
            logger.Error($"{e.Message}\n{e.StackTrace}", "error.log");
            return Results.Problem(e.Message, statusCode: 500);            
        }
    }
}