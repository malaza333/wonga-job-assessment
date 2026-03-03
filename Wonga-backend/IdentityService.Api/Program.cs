using IdentityService.Api.Clients;
using IdentityService.Api.Middleware;
using IdentityService.Application.Common.Clients;
using IdentityService.Application.Users.Services;
using IdentityService.Infrastructure;
using IdentityService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI (DB + Repos + Security)
//builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddScoped<IAuthService, AuthService>();

// Audit (HttpClient)
builder.Services.Configure<AuditOptions>(builder.Configuration.GetSection("Audit"));
builder.Services.AddHttpClient<IAuditClient, AuditClient>();

// JWT Auth
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"]!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

            // Important so ClaimTypes.NameIdentifier maps to "sub"
            NameClaimType = "sub"
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger always
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

//if (!app.Environment.IsDevelopment())
//{
    app.UseHttpsRedirection();
//}

app.UseRouting();
app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply migrations only in Development (not in Testing)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    db.Database.Migrate();
}

app.Run();

// Required for WebApplicationFactory<Program> in integration tests
public partial class Program { }