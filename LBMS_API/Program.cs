using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Models;
using LBMS_API.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/api/v1/books", async (int? id, ApplicationDbContext db) => await new BookService(db).Get(id));
app.MapPost("/api/v1/books", async (BookDTO obj, ApplicationDbContext db) => await new BookService(db).Post(obj));
app.MapPut("/api/v1/books", async (BookDTO obj, ApplicationDbContext db) => await new BookService(db).Put(obj));
app.MapDelete("/api/v1/books", async (int? id, ApplicationDbContext db) => await new BookService(db).Delete(id));

app.MapGet("/api/v1/users", async (int? id, ApplicationDbContext db) => await new UserService(db).Get(id));
app.MapPost("/api/v1/users", async (UserDTO obj, ApplicationDbContext db) => await new UserService(db).Post(obj));
app.MapPut("/api/v1/users", async (int? id, UserDTO obj, ApplicationDbContext db) => await new UserService(db).Put(id, obj));
app.MapDelete("/api/v1/users", async (int? id, ApplicationDbContext db) => await new UserService(db).Delete(id));

app.MapGet("/api/v1/loans", async (Guid? id, ApplicationDbContext db) => await new LoanService(db).Get(id));
app.MapPost("/api/v1/loans", async (LoanDTO obj, ApplicationDbContext db) => await new LoanService(db).Post(obj));
app.MapPut("/api/v1/loans", async (Guid? id, LoanDTO obj, ApplicationDbContext db) => await new LoanService(db).Put(id, obj));

app.MapGet("/api/v1/categories", async (int? id, ApplicationDbContext db) => await new CategoryService(db).Get(id));
app.MapPost("/api/v1/categories", async (CategoryDTO obj, ApplicationDbContext db) => await new CategoryService(db).Post(obj));
app.MapPut("/api/v1/categories", async (int? id, CategoryDTO obj, ApplicationDbContext db) => await new CategoryService(db).Put(id, obj));
app.MapDelete("/api/v1/categories", async (int? id, ApplicationDbContext db) => await new CategoryService(db).Delete(id));


app.Run();