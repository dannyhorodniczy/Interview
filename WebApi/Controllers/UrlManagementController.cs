using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlManagementController : ControllerBase
{

    [HttpGet]
    public string GetAndDoSomething()
    {
        return $"Is this running on Linux? {RuntimeInformation.IsOSPlatform(OSPlatform.Linux)}";
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> RedirectAsync(SqlDataSource dataSource, Guid id)
    {
        await using var command = dataSource.CreateCommand($"""
SELECT TOP 1 id, longUrl
FROM [TestDatabase].[dbo].[Urls]
WHERE id = '{id}'
""");
        var reader = await command.ExecuteReaderAsync();

        if (reader.Read())
        {
            return Redirect(reader.GetString(1));
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<string> CreateShortUrlAsync(SqlDataSource dataSource, [FromBody] EncodeUrlRequest request)
    {
        await using var command = dataSource.CreateCommand($"""
SELECT TOP 1 id
FROM [TestDatabase].[dbo].[Urls]
WHERE longUrl = '{request.url}'
""");
        var test = await command.ExecuteScalarAsync();
        if (test is Guid guid)
        {
            return BuildUrl(Request, guid);
        }

        return await EncodeAndSaveUrlAsync(dataSource, request.url);
    }

    private async Task<string> EncodeAndSaveUrlAsync(SqlDataSource dataSource, string longUrl)
    {
        var id = Guid.NewGuid();
        // insert into db
        await using var command = dataSource.CreateCommand($"""
INSERT INTO [TestDatabase].[dbo].[Urls]
VALUES ('{id}', '{longUrl}'); 
""");
        await command.ExecuteScalarAsync();

        var host = Request.Host;

        return BuildUrl(Request, id);
    }

    private static string BuildUrl(HttpRequest request, Guid id)
    {
        return $"{request.Scheme}://{request.Host}/UrlManagement/{id}";
    }
}

public record EncodeUrlRequest(string url);