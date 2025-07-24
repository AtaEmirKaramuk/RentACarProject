using Abstraction.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Email;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using RentACarProject.API.Middlewares;
using RentACarProject.Application.Abstraction.Repositories;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Behaviors;
using RentACarProject.Application.Features.Auth.Commands;
using RentACarProject.Application.Services;
using RentACarProject.Application.Validators.Auth;
using RentACarProject.Infrastructure.Configurations;
using RentACarProject.Infrastructure.Services;
using RentACarProject.Infrastructure.Services.Payments;
using RentACarProject.Persistence.Context;
using RentACarProject.Persistence.Repositories;
using RentACarProject.Persistence.Seed;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Swagger + Controllers
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

// DbContext
builder.Services.AddDbContext<RentACarDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories & Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<ICustomerRepository, EfCustomerRepository>();
builder.Services.AddScoped<ICarRepository, EfCarRepository>();
builder.Services.AddScoped<IBrandRepository, EfBrandRepository>();
builder.Services.AddScoped<IModelRepository, EfModelRepository>();
builder.Services.AddScoped<IReservationRepository, EfReservationRepository>();
builder.Services.AddScoped<IPaymentRepository, EfPaymentRepository>();
builder.Services.AddScoped<ILocationRepository, EfLocationRepository>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Şifre doğrulama ve cache servisi (PasswordChange için gerekli)
builder.Services.AddMemoryCache();

// SMTP Mail
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

builder.Services.AddHttpContextAccessor();

// JWT
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

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddFluentValidationClientsideAdapters();

var app = builder.Build();

// Admin seed işlemi
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RentACarDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    SeedData.SeedAdminUser(dbContext, configuration);

}




//builder.Services.AddHttpClient("iyzico", client =>
//{
//    client.BaseAddress = new Uri("https://api.iyzico.com"); // Gerçek URL'yi buraya yaz
//    client.DefaultRequestHeaders.Add("Authorization", "Bearer {your_api_key}"); // Gerekliyse
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//});


//builder.Services.AddScoped<IPaymentStrategyService, BankTransferPaymentService>(); // sadece örnek, factory üzerinden çözülür
////builder.Services.AddScoped<BankTransferPaymentService>();
//builder.Services.AddScoped<IyzicoPaymentService>();
//builder.Services.AddScoped<IPaymentStrategyFactory, PaymentStrategyFactory>();


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

// Middlewares
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
