using System.Text;
using Panbyte.Converters;
using Panbyte.Formats;

namespace Panbyte.InputProcessing;

/// <summary>
/// Processes the input given to the program 
/// </summary>
public class InputProcessor
{
    private readonly IConverter _converter;
    private readonly Format _outputFormat;

    public InputProcessor(IConverter converter, Format outputFormat)
    {
        _converter = converter;
        _outputFormat = outputFormat;
    }

    /// <summary>
    /// Creates a new failure function for Knuth Morris Pratt algorithm.
    /// </summary>
    /// <param name="delimiter">searched delimiter</param>
    /// <returns>KMP failure function</returns>
    private static int[] KmpTable(string delimiter)
    {
        var table = new int[delimiter.Length + 1];
        table[0] = -1;

        var candidate = 0;
        var position = 1;

        while (position < delimiter.Length)
        {
            if (delimiter[position] == delimiter[candidate])
            {
                table[position] = table[candidate];
            }
            else
            {
                table[position] = candidate;
                while (candidate >= 0 && delimiter[position] != delimiter[candidate])
                {
                    candidate = table[candidate];
                }
            }

            candidate++;
            position++;
        }

        table[position] = candidate;

        return table;
    }

    /// <summary>
    /// Implementation of Knuth Morris Pratt algorithm for finding a delimiter in a stream.
    /// </summary>
    /// <param name="delimiter">Delimiter for splitting the input.</param>
    /// <param name="reader">Input text stream.</param>
    /// <param name="writer">Output text stream.</param>
    private void ProcessWithKmp(string delimiter, TextReader reader, TextWriter writer)
    {
        StringBuilder buffer = new(); // Current buffer read from the stream

        var bufferIndex = 0; // Position in buffer
        var delimiterIndex = 0; // Position in delimiter
        var table = KmpTable(delimiter); // Build a partial match table

        if (reader.Peek() <= 0)
        {
            // Nothing to read in the stream, exit whole function
            return;
        }

        // Read first character
        buffer.Append((char)reader.Read());

        while (true)
        {
            if (delimiter[delimiterIndex] == buffer[bufferIndex])
            {
                delimiterIndex++;
                bufferIndex++;

                if (delimiterIndex == delimiter.Length)
                {
                    // Found delimiter
                    var delimiterStart = bufferIndex - delimiterIndex;

                    // Remove delimiter from the buffer
                    buffer.Remove(delimiterStart, delimiter.Length);

                    // Convert the buffer and append the delimiter
                    writer.Write(_converter.ConvertTo(buffer.ToString(), _outputFormat));
                    writer.Write(delimiter);

                    // Reset the buffer and pointers, start over
                    buffer.Clear();
                    bufferIndex = 0;
                    delimiterIndex = 0;
                }
            }
            else
            {
                delimiterIndex = table[delimiterIndex];

                if (delimiterIndex >= 0)
                {
                    // Continue to the next iteration, skip reading the next character
                    continue;
                }

                bufferIndex++;
                delimiterIndex++;
            }

            // No more characters, exit the loop
            if (reader.Peek() <= 0)
            {
                break;
            }

            buffer.Append((char)reader.Read());
        }

        // Write the last item (content after last occurrence of delimiter)
        writer.WriteLine(_converter.ConvertTo(buffer.ToString(), _outputFormat));
    }

    /// <summary>
    /// Process the input with empty delimiter (convert every character independently.
    /// </summary>
    /// <param name="reader">Input text stream.</param>
    /// <param name="writer">Output text stream.</param>
    private void ProcessEmptyDelimiter(TextReader reader, TextWriter writer)
    {
        while (reader.Peek() > 0)
        {
            writer.Write(_converter.ConvertTo(((char)reader.Read()).ToString(), _outputFormat));
        }

        writer.WriteLine();
    }

    /// <summary>
    /// Process the input without considering any delimiter.
    /// </summary>
    /// <param name="reader">Input text stream.</param>
    /// <param name="writer">Output text stream.</param>
    private void ProcessWithoutDelimiter(TextReader reader, TextWriter writer)
    {
        writer.WriteLine(_converter.ConvertTo(reader.ReadToEnd(), _outputFormat));
    }


    /// <summary>
    /// Reads the program input, splits it with given delimiter (first delimiter occurrence is taken into account,
    /// if delimiters overlap), converts the content by given converter, joins the output with the same delimiter and
    /// writes it to the output.
    /// </summary>
    /// <param name="delimiter">Delimiter (can be more than one character long).</param>
    /// <param name="inputFilePath">Input file path. If null, stdin is used.</param>
    /// <param name="outputFilePath">Output file path. If null, stdout is used.</param>
    public void ProcessInput(string? delimiter = null, string? inputFilePath = null, string? outputFilePath = null)
    {
        // Setup input and output streams (file or stdin/stdout)
        using var textReader = inputFilePath is null ? Console.In : new StreamReader(inputFilePath);
        using var textWriter = outputFilePath is null ? Console.Out : new StreamWriter(outputFilePath);

        switch (delimiter)
        {
            case null:
                // Delimiter is null -> don't take delimiter into account, process whole file
                ProcessWithoutDelimiter(textReader, textWriter);
                break;

            case "":
                // Delimiter is empty string -> convert after every char
                ProcessEmptyDelimiter(textReader, textWriter);
                break;

            default:
                // Delimiter is at least one char long -> use Knuth–Morris–Pratt algorithm
                ProcessWithKmp(delimiter, textReader, textWriter);
                break;
        }
    }
}