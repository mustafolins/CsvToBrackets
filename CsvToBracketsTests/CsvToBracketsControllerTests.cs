using CsvToBrackets.Controllers;
using CsvToBrackets.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToBracketsTests;

public  class CsvToBracketsControllerTests
{
    public CsvToBracketsController Controller { get; set; }

    [SetUp]
    public void Setup()
    {
        Controller = new(new CsvToBracketsService(new Mock<ILogger<CsvToBracketsService>>().Object), new Mock<ILogger<CsvToBracketsController>>().Object);
    }

    [Test]
    public void Post_WithValidCsv_ReturnsBrackets()
    {
        // Arrange
        var csv = "Name,Age\nJohn,30\n";

        // Act
        var result = Controller.Post(csv) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo($"[Name] [Age]{Environment.NewLine}[John] [30]{Environment.NewLine}"));
    }

    [Test]
    public void Post_WithQuotedCsv_ReturnsBrackets()
    {
        // Arrange
        var csv = "\"Name\",\"Age\"\n\"John\",\"30\"\n";

        // Act
        var result = Controller.Post(csv) as OkObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Value, Is.EqualTo($"[Name] [Age]{Environment.NewLine}[John] [30]{Environment.NewLine}"));
    }

    [Test]
    public void Post_WithInvalidCsv_ReturnsBadRequest()
    {
        // Arrange
        var csv = "Name,Age\nJohn";

        // Act
        var result = Controller.Post(csv) as ObjectResult;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));
    }
}
