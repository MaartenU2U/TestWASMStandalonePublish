using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;

namespace BlazorApp.StaticWebApp.Api;

public class WeatherForecastFunction
{
  private readonly ILogger<WeatherForecastFunction> _logger;

  public WeatherForecastFunction(ILogger<WeatherForecastFunction> logger) => _logger = logger;

  [FunctionName("WeatherForecast")]
  public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
  {
    int temp = 0;
    WeatherForecast[] result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
      TemperatureC = temp = Random.Shared.Next(-20, 55),
      Summary = GetSummary(temp)
    }).ToArray();
    return new OkObjectResult(result);
  }

  private string GetSummary(int temp)
  => temp switch
  {
    <= 0 => "Freezing",
    > 0 and < 16 => "Cold",
    >= 32 => "Hot",
    _ => "Mild"
  };

  public class WeatherForecast
  {
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public required string Summary { get; set; }
  }
}