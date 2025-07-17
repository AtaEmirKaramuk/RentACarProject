using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentACarProject.API.Middlewares;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Behaviors;
using RentACarProject.Application.Features.Auth.Commands;
using RentACarProject.Application.Services;
using RentACarProject.Application.Validators.Auth;
using RentACarProject.Persistence.Context;
using RentACarProject.Persistence.Repositories;
using RentACarProject.Persistence.Seed;
//using Serilog;
//using Serilog.Sinks.MSSqlServer;
using System.Text;
using RentACarProject.Application.Abstraction.Services;



var builder = WebApplication.CreateBuilder(args);

//// ✅ Serilog logger kurulumu
//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(builder.Configuration)
//    .Enrich.FromLogContext()
//    .WriteTo.Console()
//    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
//    .WriteTo.MSSqlServer(
//        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
//        sinkOptions: new MSSqlServerSinkOptions
//        {
//            TableName = "Logs",
//            AutoCreateSqlTable = true
//        })
//    .CreateLogger();

//builder.Host.UseSerilog();

// Controllers ve Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("Admin", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Admin API",
        Version = "v1"
    });

    options.SwaggerDoc("Public", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Public API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Bearer token kullanımı için.  
                        Authorization: Bearer {token} şeklinde giriniz.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

//  DbContext
builder.Services.AddDbContext<RentACarDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  Repositories & services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<ICarRepository, EfCarRepository>();
builder.Services.AddScoped<IBrandRepository, EfBrandRepository>();
builder.Services.AddScoped<IModelRepository, EfModelRepository>();
builder.Services.AddScoped<IReservationRepository, EfReservationRepository>();
builder.Services.AddScoped<IPaymentRepository, EfPaymentRepository>();


//  JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "DEFAULT_SECRET_KEY";
var key = Encoding.ASCII.GetBytes(jwtKey);
builder.Services.AddSingleton(new JwtTokenService(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

//  MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly));

//  FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

//  Admin seed
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RentACarDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    SeedData.SeedAdminUser(dbContext, configuration);
}

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin API");
        options.SwaggerEndpoint("/swagger/Public/swagger.json", "Public API");
    });
}

//  Middlewares
// app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
