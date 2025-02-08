using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Models;
using Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBMS_API.Services;

public class UserService(HttpContext httpContext, ApplicationDbContext db) {
    private readonly Logger logger = Logger.Default;

    [HttpGet]
    public async Task<IResult> Get(int? id) {        
        PathString path = httpContext.Request.Path;
        IQueryable<User> query = db.Users.AsQueryable();
    
        if (id != null) {
            query = query.Where(b => b.ID == id);
            User? user = await query.FirstOrDefaultAsync();

            if (user != null) {
                logger.Information($"HTTP/1.1 GET - 200 {path}/{user.ID}");
                return Results.Ok(user); 
            }
            logger.Information($"HTTP/1.1 GET - 404 {path}/{user.ID}");
            return Results.NotFound();
        }
    
        List<User> users = await query.ToListAsync();
        if (users.Any()) {
            logger.Information($"HTTP/1.1 GET - 200 {path}");
            return Results.Ok(users); 
        }
        logger.Information($"HTTP/1.1 GET - 404 {path}");
        return Results.NotFound();
    }
    
    [HttpPut]
    public async Task<IResult> Put(int? id, UserDTO obj) {
        PathString path = httpContext.Request.Path;
        User? user = await db.Users.FindAsync(id);

        if (user == null) {
            logger.Information($"HTTP/1.1 PUT - 404 {path}/{id}");
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
        
        logger.Information($"HTTP/1.1 PUT - 201 {path}/{user.ID}");
        return Results.Ok();
    }
    
    [HttpPost]
    public async Task<IResult> Post(UserDTO obj) {
        PathString path = httpContext.Request.Path;
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
                logger.Information($"HTTP/1.1 POST - 201 {path}/{employee.ID}");
                return Results.Created();
            } catch (Exception e) {
                logger.Information($"HTTP/1.1 POST - 500 {path} - {e.Message}");
                logger.Error($"{e.Message}\n{e.StackTrace}", "error.log");
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
            logger.Information($"HTTP/1.1 POST - 201 {path}/{patron.ID}");
            return Results.Created();
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
            User? user = await db.Users.FindAsync(id);

            if (user == null) {
                logger.Information($"HTTP/1.1 DELETE - 404 {path}/{id}");
                return Results.NotFound();
            }
            
            db.Remove(user);
            await db.SaveChangesAsync();
            logger.Information($"HTTP/1.1 POST - 201 {path}/{user.ID}");
            return Results.Ok();
        }
        catch (Exception e) {
            logger.Information($"HTTP/1.1 POST - 500 {path} - {e.Message}");
            logger.Error($"{e.Message}\n{e.StackTrace}", "error.log"); 
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
}