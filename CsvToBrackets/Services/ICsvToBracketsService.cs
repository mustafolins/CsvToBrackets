using System.Text;

namespace CsvToBrackets.Services;

public interface ICsvToBracketsService
{
    public string ToBrackets(string csv);
}