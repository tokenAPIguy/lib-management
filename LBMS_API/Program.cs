using LBMS_API.Data;
using LBMS_API.Data.DTO;
using LBMS_API.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

Logging.Logger logger = Logging.Logger.Default;
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

app.MapGet("/api/v1/books", async (int? id, HttpContext httpContext, ApplicationDbContext db) => await new BookService(httpContext, db).Get(id));
app.MapPost("/api/v1/books", async (BookDTO obj, HttpContext httpContext,ApplicationDbContext db) => await new BookService(httpContext, db).Post(obj));
app.MapPut("/api/v1/books", async (BookDTO obj, HttpContext httpContext,ApplicationDbContext db) => await new BookService(httpContext, db).Put(obj));
app.MapDelete("/api/v1/books", async (int? id, HttpContext httpContext, ApplicationDbContext db) => await new BookService(httpContext, db).Delete(id));

app.MapGet("/api/v1/users", async (int? id, HttpContext httpContext, ApplicationDbContext db) => await new UserService(httpContext, db).Get(id));
app.MapPost("/api/v1/users", async (UserDTO obj, HttpContext httpContext, ApplicationDbContext db) => await new UserService(httpContext, db).Post(obj));
app.MapPut("/api/v1/users", async (int? id, UserDTO obj, HttpContext httpContext, ApplicationDbContext db) => await new UserService(httpContext, db).Put(id, obj));
app.MapDelete("/api/v1/users", async (int? id, HttpContext httpContext, ApplicationDbContext db) => await new UserService(httpContext, db).Delete(id));

app.MapGet("/api/v1/loans", async (Guid? id, HttpContext httpContext, ApplicationDbContext db) => await new LoanService(httpContext, db).Get(id));
app.MapPost("/api/v1/loans", async (LoanDTO obj, HttpContext httpContext, ApplicationDbContext db) => await new LoanService(httpContext, db).Post(obj));
app.MapPut("/api/v1/loans", async (Guid? id, LoanDTO obj, HttpContext httpContext, ApplicationDbContext db) => await new LoanService(httpContext, db).Put(id, obj));

app.MapGet("/api/v1/categories", async (int? id, HttpContext httpContext, ApplicationDbContext db) => await new CategoryService(httpContext, db).Get(id));
app.MapPost("/api/v1/categories", async (CategoryDTO obj, HttpContext httpContext, ApplicationDbContext db) => await new CategoryService(httpContext, db).Post(obj));
app.MapPut("/api/v1/categories", async (int? id, CategoryDTO obj, HttpContext httpContext, ApplicationDbContext db) => await new CategoryService(httpContext, db).Put(id, obj));
app.MapDelete("/api/v1/categories", async (int? id, HttpContext httpContext, ApplicationDbContext db) => await new CategoryService(httpContext, db).Delete(id));

app.Run();