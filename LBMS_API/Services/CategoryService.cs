using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Services;

public class CategoryService(ApplicationDbContext db) {
    [HttpGet]
    public async Task<IResult> Get(int? id) {
        IQueryable<Category> query = db.Categories.AsQueryable();

        if (id != null) {
            query = query.Where(c => c.ID == id);
            Category? category = await query.FirstOrDefaultAsync();
            return category != null
                ? Results.Ok(category)
                : Results.NotFound();
        }
    
        List<Category> categories = await query.ToListAsync();
        return categories.Any()
            ? Results.Ok(categories)
            : Results.NotFound();
    }

    [HttpPost]
    public async Task<IResult> Post(CategoryDTO obj) {
        try {
            Category category = new() {
                Name = obj.Name,
                CanBeMainCategory = obj.CanBeMainCategory
            };

            db.Add(category);
            await db.SaveChangesAsync();
            return Results.Created();
        }
        catch (Exception e) {
            // log e.stacktrace
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
    
    [HttpPut]
    public async Task<IResult> Put(int? id, CategoryDTO obj) {
        try {
            Category? category = await db.Categories.FindAsync(id);

            if (category == null) {
                return Results.NotFound();
            }
            
            category.CanBeMainCategory = obj.CanBeMainCategory;
            db.Update(category);
            await db.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception e) {
            return Results.Problem(e.Message, statusCode: 500);
        }

    }

    [HttpDelete]
    public async Task<IResult> Delete(int? id) {
        try {
            Category? category = await db.Categories.FindAsync(id);

            if (category == null) {
                return Results.NotFound();
            }
            
            bool categoryIsInUse = await db.Books.AnyAsync(b => b.CategoryID == id);
            if (categoryIsInUse) {
                return Results.BadRequest("Category is in use.");
            }
            
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception e) {
            return Results.Problem(e.Message, statusCode: 500);
        }
    }


}