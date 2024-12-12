using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly Dictionary<Guid, string> _shortToLong = new Dictionary<Guid, string>();
    private static readonly Dictionary<string, Guid> _longToShort = new Dictionary<string, Guid>();

    public WeatherForecastController(FileManager fileManager)
    {
    }

    // https://localhost:7267/WeatherForecast
    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        var idString = this.Request.QueryString.Value.Replace('?', ' ');
        var id = Guid.Parse(idString);

        if (_shortToLong.TryGetValue(id, out var longUrl))
        {
            return Redirect(longUrl);
        }

        return NotFound();
    }

    // https://localhost:7267/WeatherForecast
    [HttpPost(Name = "PostWeatherForecast")]
    public string Post(Request request)
    {
        if (_longToShort.TryGetValue(request.longUrl, out Guid id))
        {
            return $"https://localhost:7267/WeatherForecast?{id}";
        }

        return EncodeInternal(request.longUrl);
    }

    private string EncodeInternal(string longUrl)
    {
        var id = Guid.NewGuid();
        _shortToLong[id] = longUrl;
        _longToShort[longUrl] = id;
        return $"https://localhost:7267/WeatherForecast?{id}";
    }
}

public record Request(string longUrl);
