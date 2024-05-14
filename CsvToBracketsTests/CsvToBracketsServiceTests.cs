using CsvToBrackets.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace CsvToBracketsTests;

public class CsvToBracketsServiceTests
{
    public CsvToBracketsService Service { get; set; }

    [SetUp]
    public void Setup()
    {
        Service = new(new Mock<ILogger<CsvToBracketsService>>().Object);
    }

    [Test]
    public void ToBrackets_WithValidCsv_ReturnsBrackets()
    {
        // Arrange
        var csv = "Name,Age\nJohn,30\n";

        // Act
        var brackets = Service.ToBrackets(csv);

        // Assert
        Assert.That(brackets, Is.EqualTo($"[Name] [Age]{Environment.NewLine}[John] [30]{Environment.NewLine}"));
    }

    [Test]
    public void ToBrackets_WithQuotedCsv_ReturnsBrackets()
    {
        // Arrange
        var csv = "\"Name\",\"Age\"\n\"John\",\"30\"\n";

        // Act
        var brackets = Service.ToBrackets(csv);

        // Assert
        Assert.That(brackets, Is.EqualTo($"[Name] [Age]{Environment.NewLine}[John] [30]{Environment.NewLine}"));
    }

    [Test]
    public void ToBrackets_WithInvalidCsv_ThrowsFormatException()
    {
        // Arrange
        var csv = "Name,Age\nJohn";

        // Act
        void Act() => Service.ToBrackets(csv);

        // Assert
        Assert.That(Act, Throws.InstanceOf<FormatException>());
    }
}