using System.Text;

namespace CsvToBrackets.Services;

public class CsvToBracketsService(ILogger<CsvToBracketsService> logger)
{
    public ILogger<CsvToBracketsService> Logger { get; } = logger;

    public string ToBrackets(string csv)
    {
        var lines = csv.Split("\n");
        var brackets = new StringBuilder();
        // Get the first line of the CSV
        var header = lines[0];
        var columns = GetColumns(header.AsEnumerable());
        // Process the header
        var headerCount = ProcessHeader(brackets, columns);
        // Process the body
        ProcessBody(lines.Skip(1), brackets, headerCount);
        // Return the brackets
        return brackets.ToString();
    }

    public int ProcessHeader(StringBuilder brackets, IEnumerable<string> columns)
    {
        int headerCount = 0;

        // Loop through the header columns
        foreach (var column in columns)
        {
            brackets.Append(column);
            headerCount++;
        }
        brackets.AppendLine();

        return headerCount;
    }

    public void ProcessBody(IEnumerable<string> lines, StringBuilder brackets, int headerCount)
    {
        // Loop through other lines in the CSV
        foreach (var line in lines)
        {
            var columnCount = 0;
            var columns = GetColumns(line.AsEnumerable());
            foreach (var column in columns)
            {
                brackets.Append(column);
                columnCount++;
            }
            // Check if the number of columns in the CSV matches the header
            if (columnCount != headerCount)
            {
                throw new FormatException("Number of columns in the CSV does not match the header");
            }
            brackets.AppendLine();
        }
    }

    /// <summary>
    /// Gets the columns from a CSV line.
    /// </summary>
    /// <param name="enumerable">The line as an <see cref="IEnumerable{char}"/>.</param>
    /// <returns>An <see cref="IEnumerable{string}"/> of the parsed columns.</returns>
    public IEnumerable<string> GetColumns(IEnumerable<char> enumerable)
    {
        var column = new StringBuilder();
        var inQuotes = false;
        foreach (var c in enumerable)
        {
            // If we encounter a quote, toggle the inQuotes flag
            if (c == '"')
            {
                inQuotes = !inQuotes;

                // If inQuotes is true then we're at the start of a quoted column
                if (inQuotes)
                {
                    column.Append('[');
                }
                // If inQuotes is false then we're at the end of a quoted column
                else
                {
                    column.Append(']');
                }
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
