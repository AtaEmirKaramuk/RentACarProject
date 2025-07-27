using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RentACarProject.API.Middlewares;
using RentACarProject.Application.Abstraction.Services;
using RentACarProject.Application.Behaviors;
using RentACarProject.Application.Features.Auth.Commands;
using RentACarProject.Application.Settings;
using RentACarProject.Application.Validators.Auth;
using RentACarProject.Infrastructure.DependencyInjection;
using RentACarProject.Infrastructure.Services;
using RentACarProject.Persistence.Context;
using RentACarProject.Persistence.DependencyInjection;
using RentACarProject.Persistence.Seed;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Serilog yapılandırması
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});


// 🔹 Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("Admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
    options.SwaggerDoc("Public", new OpenApiInfo { Title = "Public API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Bearer token. Örn: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// 🔹 DbContext
builder.Services.AddDbContext<RentACarDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 JWT Settings & Service Registration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// 🔹 Service Registrations
builder.Services.AddRepositories();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

// 🔹 FluentValidation & MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddFluentValidationClientsideAdapters();

// 🔹 Common services
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// 🔹 Build app
var app = builder.Build();

// 🔹 Seed admin
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RentACarDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    SeedData.SeedAdminUser(dbContext, configuration);
}

// 🔹 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin API");
        options.SwaggerEndpoint("/swagger/Public/swagger.json", "Public API");
    });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// 🔹 Başlangıç logu
Log.Information("Uygulama başarıyla başlatıldı.");
app.Run();

// 🔧 MSSQL Log ColumnOptions
ColumnOptions GetSqlColumnOptions()
{
    var columnOptions = new ColumnOptions();

    columnOptions.Store.Remove(StandardColumn.Properties);
    columnOptions.Store.Remove(StandardColumn.MessageTemplate);

    columnOptions.AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn("Path", SqlDbType.NVarChar, dataLength: 300),
        new SqlColumn("Method", SqlDbType.NVarChar, dataLength: 10),
        new SqlColumn("StatusCode", SqlDbType.Int),
        new SqlColumn("RequestBody", SqlDbType.NVarChar, dataLength: -1),
        new SqlColumn("ResponseBody", SqlDbType.NVarChar, dataLength: -1),
        new SqlColumn("IpAddress", SqlDbType.NVarChar, dataLength: 50),
        new SqlColumn("UserAgent", SqlDbType.NVarChar, dataLength: 300),
        new SqlColumn("ResponseTimeMs", SqlDbType.BigInt),
        new SqlColumn("UserId", SqlDbType.NVarChar, dataLength: 100),
        new SqlColumn("TraceId", SqlDbType.NVarChar, dataLength: 100),
    };

    return columnOptions;
}
