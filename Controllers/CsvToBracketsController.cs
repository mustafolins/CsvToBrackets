using CsvToBrackets.Services;
using Microsoft.AspNetCore.Mvc;

namespace CsvToBrackets.Controllers;

[ApiController]
[Route("[controller]")]
public class CsvToBracketsController : ControllerBase
{
    [HttpPost]
    //[Consumes("text/csv")]
    public IActionResult Post([FromBody]string csv)
    {
        var brackets = CsvToBracketsService.CsvToBrackets(csv);
        return Ok(brackets);
    }
}
