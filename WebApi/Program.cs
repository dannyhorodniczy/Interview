
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // add SQL
        //builder.Services.AddSqlDataSource("Server=localhost\\SQLEXPRESS;Database=TestDatabase;Trusted_Connection=True;TrustServerCertificate=True");
        //builder.Services.AddSqlDataSource("Server=192.168.2.44;Database=TestDatabase;User ID=sa;Password=This_Is_@_?pw23;Trusted_Connection=True;TrustServerCertificate=True");
        //builder.Services.AddSqlDataSource("Server=192.168.2.44;Database=TestDatabase;User ID=sa;Password=This_Is_@_?pw23;TrustServerCertificate=True",
        //    // remove sql logging, can be configured via appsettings
        //    setupAction => setupAction.UseLoggerFactory(null)
        //    );

        // add postgres
        builder.Services.AddNpgsqlDataSource("Host=localhost;Database=urlshortener;Username=postgres_user;Password=postgres_password");

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
