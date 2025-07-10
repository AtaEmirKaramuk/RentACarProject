using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Features.Auth.Commands;
using RentACarProject.Application.Services;
using RentACarProject.Application.Validators.Auth;
using RentACarProject.Persistence.Context;
using RentACarProject.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RentACarDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Scoped servisler
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();

// JWT Key ayarı
var jwtKey = builder.Configuration["Jwt:Key"] ?? "DEFAULT_SECRET_KEY";
builder.Services.AddSingleton(new JwtTokenService(jwtKey));

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly));

// ✅ FluentValidation (yeni yöntem)
builder.Services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
