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
        try {
            User user = new() {
                FirstName = obj.FirstName,
                MiddleInitial = obj.MiddleInitial ?? null,
                LastName = obj.LastName,
                UserName = obj.UserName,
                Email = obj.Email,
                BirthDate = obj.BirthDate,
                Address = obj.Address
            };
            
            db.Add(user);
            await db.SaveChangesAsync();
            return Results.Created();
        } 
        catch (Exception e) {
            // log e.stacktrace
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
}