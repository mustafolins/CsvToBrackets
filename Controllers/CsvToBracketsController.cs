using CsvToBrackets.Services;
using Microsoft.AspNetCore.Mvc;

namespace CsvToBrackets.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvToBracketsController(CsvToBracketsService csvToBracketsService, ILogger<CsvToBracketsController> logger) : ControllerBase
{
    public ILogger<CsvToBracketsController> Logger { get; } = logger;
    public CsvToBracketsService BracketService { get; } = csvToBracketsService;

    [HttpPost]
    //[Consumes("text/csv")]
    public IActionResult Post([FromBody]string csv)
    {
        Logger.LogInformation("CSV: {csv}", csv);

        var brackets = BracketService.ToBrackets(csv);

        Logger.LogInformation("Converted CSV to brackets: {brackets}", brackets);
        return Ok(brackets);
    }
}
