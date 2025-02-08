using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Models;
using Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Services;

public class CategoryService(HttpContext httpContext ,ApplicationDbContext db) {
    private readonly Logger logger = Logger.Default;

    [HttpGet]
    public async Task<IResult> Get(int? id) {
        PathString path = httpContext.Request.Path;
        IQueryable<Category> query = db.Categories.AsQueryable();

        if (id != null) {
            query = query.Where(c => c.ID == id);
            Category? category = await query.FirstOrDefaultAsync();
            if (category != null) {
                logger.Information($"HTTP/1.1 GET - 200 {path}/{category.ID}");
                return Results.Ok(category);
            }
            logger.Information($"HTTP/1.1 GET - 404 {path}/{category.ID}");
            return Results.NotFound();
        }
    
        List<Category> categories = await query.ToListAsync();
        if (categories.Any()) {
            logger.Information($"HTTP/1.1 GET - 200 {path}");
            return Results.Ok(categories);
        } 
        logger.Information($"HTTP/1.1 GET - 404 {path}");
        return Results.NotFound();
    }

    [HttpPost]
    public async Task<IResult> Post(CategoryDTO obj) {
        PathString path = httpContext.Request.Path;
        try {
            Category category = new() {
                Name = obj.Name,
                CanBeMainCategory = obj.CanBeMainCategory
            };

            db.Add(category);
            await db.SaveChangesAsync();
            logger.Information($"HTTP/1.1 POST - 201 {path}/{category.ID}");
            return Results.Created();
        }
        catch (Exception e) {
            logger.Information($"HTTP/1.1 POST - 500 {path} - {e.Message}");
            logger.Error($"{e.Message}\n{e.StackTrace}", "error.log");            
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
    
    [HttpPut]
    public async Task<IResult> Put(int? id, CategoryDTO obj) {
        PathString path = httpContext.Request.Path;
        try {
            Category? category = await db.Categories.FindAsync(id);

            if (category == null) {
                logger.Information($"HTTP/1.1 GET - 404 {path}");
                return Results.NotFound();
            }
            
            category.CanBeMainCategory = obj.CanBeMainCategory;
            db.Update(category);
            await db.SaveChangesAsync();
            logger.Information($"HTTP/1.1 PUT - 200 {path} | {obj.Name}");
            return Results.Ok();
        }
        catch (Exception e) {
            logger.Information($"HTTP/1.1 POST - 500 {path} - {e.Message}");
            logger.Error($"{e.Message}\n{e.StackTrace}", "error.log");
            return Results.Problem(e.Message, statusCode: 500);
        }

    }

    [HttpDelete]
    public async Task<IResult> Delete(int? id) {
        PathString path = httpContext.Request.Path;
        try {
            Category? category = await db.Categories.FindAsync(id);

            if (category == null) {
                logger.Information($"HTTP/1.1 GET - 404 {path}");
                return Results.NotFound();
            }
            
            bool categoryIsInUse = await db.Books.AnyAsync(b => b.CategoryID == id);
            if (categoryIsInUse) {
                logger.Warning($"HTTP/1.1 DELETE - 400 {path}");
                return Results.BadRequest("Category is in use.");
            }
            
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            logger.Information($"HTTP/1.1 DELETE - 200 {path}/{category.ID}");
            return Results.Ok();
        }
        catch (Exception e) {
            logger.Information($"HTTP/1.1 POST - 500 {path} - {e.Message}");
            logger.Error($"{e.Message}\n{e.StackTrace}", "error.log");
            return Results.Problem(e.Message, statusCode: 500);
        }
    }


}