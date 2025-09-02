using BooksApp.API.Middlewares;
using BooksApp.Data;
using BooksApp.Data.Entities;
using BooksApp.Data.Implementation;
using BooksApp.Data.Interfaces;
using BooksApp.Services;
using BooksApp.Services.Implementation;
using BooksApp.Services.Interfaces;
using BooksApp.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(
                opt =>
                {
                    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                }
);

builder.Services.Configure<JWTConfig>(configuration.GetSection("JWTConfig"));
//builder.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(BusinessProfile)));

builder.Services.AddIdentity<UserEntity, IdentityRole>(
                    opt =>
                    {
                        opt.User.RequireUniqueEmail = true;
                    }
                )
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
