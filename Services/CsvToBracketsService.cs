using System.Text;

namespace CsvToBrackets.Services;

public class CsvToBracketsService(ILogger<CsvToBracketsService> logger)
{
    public ILogger<CsvToBracketsService> Logger { get; } = logger;

    public string ToBrackets(string csv)
    {
        Logger.LogInformation("Converting CSV to brackets");

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
        Logger.LogInformation("Processing CSV header");

        int headerCount = 0;

        // Loop through the header columns
        foreach (var column in columns)
        {
            brackets.Append(column);
            headerCount++;
        }
        brackets.AppendLine();

        Logger.LogInformation("Header count: {headerCount}", headerCount);
        return headerCount;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lines"></param>
    /// <param name="brackets"></param>
    /// <param name="headerCount"></param>
    /// <exception cref="FormatException">Thrown when the number of columns in the CSV does not match the header.</exception>
    public void ProcessBody(IEnumerable<string> lines, StringBuilder brackets, int headerCount)
    {
        Logger.LogInformation("Processing CSV body");
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
                Logger.LogCritical("Number of columns in the CSV does not match the header");
                // Throw a FormatException if the number of columns in the CSV does not match the header
                // this can happen if the CSV is malformed or misaligned
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
        Logger.LogInformation("Getting columns from CSV line");

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
