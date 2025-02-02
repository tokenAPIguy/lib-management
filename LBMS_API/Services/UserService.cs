using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Services;

public class UserService(ApplicationDbContext db) {
    [HttpGet]
    public async Task<IResult> Get(int? id) {
        IQueryable<User> query = db.Users.AsQueryable();
    
        if (id != null) {
            query = query.Where(b => b.ID == id);
            User? user = await query.FirstOrDefaultAsync();
            return user != null 
                ? Results.Ok(user) 
                : Results.NotFound();
        }
    
        List<User> users = await query.ToListAsync();
        return users.Any() 
            ? Results.Ok(users) 
            : Results.NotFound();
    }
    
    [HttpPost]
    public async Task<IResult> Post(UserDTO obj) {
        
        if (obj.Role == UserRole.Employee || obj.Role == UserRole.Admin) {
            try {
                Employee employee = new() {
                    Role =  obj.Role,
                    FirstName = obj.FirstName,
                    MiddleInitial = obj.MiddleInitial ?? null,
                    LastName = obj.LastName,
                    UserName = obj.UserName,
                    Email = obj.Email,
                    BirthDate = obj.BirthDate,
                    Address = obj.Address,
                    AccountCreationDate = DateOnly.FromDateTime(DateTime.Now),
                    Discriminator = "EMPLOYEE",
                    IsAdmin = obj.Role == UserRole.Admin 
                };
            
                db.Add(employee);
                await db.SaveChangesAsync();
                return Results.Created();
            } catch (Exception e) {
                // log e.stacktrace
                return Results.Problem(e.Message, statusCode: 500);
            }
        }
        
        try {
            Patron patron = new() {
                Role = UserRole.Patron,
                FirstName = obj.FirstName,
                MiddleInitial = obj.MiddleInitial ?? null,
                LastName = obj.LastName,
                UserName = obj.UserName,
                Email = obj.Email,
                BirthDate = obj.BirthDate,
                Address = obj.Address,
                Discriminator = "PATRON"
            };
        
            db.Add(patron);
            await db.SaveChangesAsync();
            return Results.Created();
        } 
        catch (Exception e) {
            // log e.stacktrace
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
}