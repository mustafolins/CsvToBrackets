using System.Text;

namespace CsvToBrackets.Services;

public class CsvToBracketsService
{
    public static string CsvToBrackets(string csv)
    {
        var lines = csv.Split("\n");
        var brackets = new StringBuilder();
        // Get the first line of the CSV
        var header = lines[0];
        var columns = GetColumns(header.AsEnumerable());

        // Loop through the columns
        foreach (var column in columns)
        {
            brackets.Append($"[{column}]");
        }
        brackets.AppendLine();

        // Loop through other lines in the CSV
        foreach (var line in lines.Skip(1))
        {
            columns = GetColumns(line.AsEnumerable());
            foreach (var column in columns)
            {
                brackets.Append($"[{column}]");
            }
            brackets.AppendLine();
        }
        return brackets.ToString();
    }

    public static IEnumerable<string> GetColumns(IEnumerable<char> enumerable)
    {
        var column = new StringBuilder();
        var inQuotes = false;
        foreach (var c in enumerable)
        {
            // If we encounter a quote, toggle the inQuotes flag
            if (c == '"')
            {
                inQuotes = !inQuotes;

                // Don't add the quote to the current column
                continue;
            }

            // If we encounter a comma and we're not in quotes, yield the current column
            if (c == ',' && !inQuotes)
            {
                yield return column.ToString();
                // Reset the column
                column = new();
            }
            // Otherwise, add the character to the current column
            else
            {
                column.Append(c);
            }
        }
        yield return column.ToString();
    }
}
