using CsvToBrackets.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CsvToBrackets.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvToBracketsController(CsvToBracketsService csvToBracketsService, ILogger<CsvToBracketsController> logger) : ControllerBase
{
    public ILogger<CsvToBracketsController> Logger { get; } = logger;
    public CsvToBracketsService BracketService { get; } = csvToBracketsService;

    /// <summary>
    /// Converts the given CSV to brackets.
    /// </summary>
    /// <param name="csv">The CSV to convert to brackets.</param>
    /// <returns>The CSV converted to brackets.</returns>
    /// <response code="200">Returns the CSV converted to brackets.</response>
    /// <response code="400">If the CSV is invalid.</response>
    /// <response code="500">If an error occurs while converting the CSV to brackets.</response>
    [HttpPost]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public IActionResult Post([FromBody]string csv)
    {
        Logger.LogInformation("CSV: {csv}", csv);

        try
        {
            var brackets = BracketService.ToBrackets(csv);

            Logger.LogInformation("Converted CSV to brackets: {brackets}", brackets);
            return Ok(brackets);
        }
        catch (FormatException ex)
        {
            Logger.LogError(ex, "Error converting CSV to brackets");
            return Problem(ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error converting CSV to brackets");
            return Problem("Error converting CSV to brackets", statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}
