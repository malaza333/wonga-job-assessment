
using AuditService.Api.Persistence;
using IdentityService.Api.Middleware;
using Microsoft.EntityFrameworkCore;


namespace AuditService.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Db
        builder.Services.AddDbContext<AuditDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Swagger always
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.MapControllers();


        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AuditDbContext>();
            db.Database.Migrate();
        }

        app.Run();
    }
}
