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
                    Discriminator = obj.Role == UserRole.Employee ? "EMPLOYEE" : "ADMIN",
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

    [HttpPut]
    public async Task<IResult> Put(int? id, UserDTO obj) {
        User? user = await db.Users.FindAsync(id);

        if (user == null) {
            return Results.NotFound();
        }
        
        user.FirstName = obj.FirstName;
        user.MiddleInitial = obj.MiddleInitial;
        user.LastName = obj.LastName;
        user.Email = obj.Email;
        user.Address = obj.Address;
        user.Role = obj.Role;
        user.Discriminator = obj.Role == UserRole.Employee || obj.Role == UserRole.Admin ? "EMPLOYEE" : "PATRON";
        
        db.Update(user);
        await db.SaveChangesAsync();
        return Results.Ok();
    }
    
    [HttpDelete]
    public async Task<IResult> Delete(int? id) {
        try {
            User? user = await db.Users.FindAsync(id);

            if (user == null) {
                return Results.NotFound();
            }
            
            db.Remove(user);
            await db.SaveChangesAsync();
            return Results.Ok();
        }
        catch (Exception e) {
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
}