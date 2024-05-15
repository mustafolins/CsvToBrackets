using System.Text;

namespace CsvToBrackets.Services;

public class CsvToBracketsService(ILogger<CsvToBracketsService> logger) : ICsvToBracketsService
{
    public ILogger<CsvToBracketsService> Logger { get; } = logger;

    /// <summary>
    /// Converts the given <paramref name="csv"/> to brackets.
    /// </summary>
    /// <param name="csv">The comma-seperated values.</param>
    /// <returns>The <paramref name="csv"/> converted to brackets.</returns>
    public string ToBrackets(string csv)
    {
        Logger.LogInformation("Converting CSV to brackets");

        var lines = csv.Replace("\r", "").Split("\n");
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="brackets"></param>
    /// <param name="columns"></param>
    /// <returns></returns>
    public int ProcessHeader(StringBuilder brackets, IEnumerable<string> columns)
    {
        Logger.LogInformation("Processing CSV header");

        // Add the header columns to the brackets
        var headerCount = AddColumns(brackets, columns);
        brackets.AppendLine();

        Logger.LogInformation("Header count: {headerCount}", headerCount);
        return headerCount;
    }

    /// <summary>
    /// Adds <paramref name="columns"/> to the <paramref name="brackets"/>.
    /// </summary>
    /// <param name="brackets">The <see cref="StringBuilder"/> to add <paramref name="columns"/> to.</param>
    /// <param name="columns">The columns to add to <paramref name="brackets"/>.</param>
    /// <returns>The added columns count.</returns>
    public int AddColumns(StringBuilder brackets, IEnumerable<string> columns)
    {
        int count = 0;

        // Loop through the header columns
        bool isFirst = true;
        foreach (var column in columns)
        {
            // If this is the first column, don't add a space
            if (isFirst)
            {
                isFirst = false;
            }
            else
            {
                brackets.Append(' ');
            }
            // Append the column to the brackets
            brackets.Append(column);
            count++;
        }

        // Return the number of columns added
        return count;
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
            var columns = GetColumns(line.AsEnumerable());
            var columnCount = AddColumns(brackets, columns);
            if (columnCount > 0)
            {
                // Check if the number of columns in the CSV matches the header
                if (columnCount != 0 && columnCount != headerCount)
                {
                    Logger.LogCritical("Number of columns in the CSV does not match the header");
                    // Throw a FormatException if the number of columns in the CSV does not match the header
                    // this can happen if the CSV is malformed or misaligned
                    throw new FormatException("Number of columns in the CSV does not match the header");
                }
                brackets.AppendLine();
            }
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
        foreach (var ch in enumerable)
        {
            // If we encounter a quote, toggle the inQuotes flag
            if (ch == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            // If we encounter a comma and we're not in quotes, yield the current column
            if (ch == ',' && !inQuotes)
            {
                // Yield the column and add brackets around it
                yield return $"[{column}]";
                // Reset the column
                column = new();
            }
            // Otherwise, add the character to the current column
            else
            {
                column.Append(ch);
            }
        }
        // Yield the column and add brackets around it if it's not empty
        if (column.Length > 0)
        {
            yield return $"[{column}]";
        }
        else
        {
            yield break;
        }
    }
}
