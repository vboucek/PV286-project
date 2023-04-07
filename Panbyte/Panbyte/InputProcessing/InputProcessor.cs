using System.Text;
using Panbyte.Converters;
using Panbyte.Formats;

namespace Panbyte.InputProcessing;

/// <summary>
/// Processes the input given to the program.
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
    /// <param name="delimiter">Searched delimiter.</param>
    /// <returns>KMP failure function.</returns>
    private static int[] KmpTable(byte[] delimiter)
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
    /// Implementation of Knuth Morris Pratt algorithm for finding a delimiter in a byte stream.
    /// </summary>
    /// <param name="delimiter">Delimiter for splitting the input.</param>
    /// <param name="inputStream">Input stream.</param>
    /// <param name="outputStream">Output stream.</param>
    private void ProcessWithKmp(byte[] delimiter, Stream inputStream, Stream outputStream)
    {
        List<byte> buffer = new(); // Current buffer read from the stream

        var bufferIndex = 0; // Position in the buffer
        var delimiterIndex = 0; // Position in the delimiter
        var table = KmpTable(delimiter); // Build a partial match table
        var currentByte = inputStream.ReadByte();

        if (currentByte == -1)
        {
            // Nothing to read in the stream, exit whole function
            return;
        }

        // Read first byte
        buffer.Add(Convert.ToByte(currentByte));

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
                    buffer.RemoveRange(delimiterStart, delimiter.Length);

                    // Convert the buffer and append the delimiter
                    outputStream.Write(_converter.ConvertTo(buffer.ToArray(), _outputFormat));
                    outputStream.Write(delimiter);

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

            currentByte = inputStream.ReadByte();

            // No more bytes, exit the loop
            if (currentByte == -1)
            {
                break;
            }

            buffer.Add(Convert.ToByte(currentByte));
        }

        // Write the last item (content after last occurrence of the delimiter)
        outputStream.Write(_converter.ConvertTo(buffer.ToArray(), _outputFormat));
    }

    /// <summary>
    /// Processes the input with empty delimiter (convert every byte independently).
    /// </summary>
    /// <param name="inputStream">Input stream.</param>
    /// <param name="outputStream">Output stream.</param>
    private void ProcessEmptyDelimiter(Stream inputStream, Stream outputStream)
    {
        while (true)
        {
            var currentByte = inputStream.ReadByte();

            if (currentByte == -1)
            {
                break;
            }

            outputStream.Write(_converter.ConvertTo(new[] { Convert.ToByte(currentByte) }, _outputFormat));
        }
    }

    /// <summary>
    /// Processes the input without considering any delimiter.
    /// </summary>
    /// <param name="inputStream">Input stream.</param>
    /// <param name="outputStream">Output stream.</param>
    private void ProcessWithoutDelimiter(Stream inputStream, Stream outputStream)
    {
        List<byte> buffer = new();
        
        while (true)
        {
            var currentByte = inputStream.ReadByte();

            if (currentByte == -1)
            {
                break;
            }

            buffer.Add(Convert.ToByte(currentByte));
        }
        
        outputStream.Write(_converter.ConvertTo(buffer.ToArray(), _outputFormat));
    }


    /// <summary>
    /// Reads the program byte input, splits it with given delimiter (first delimiter occurrence is taken into account,
    /// if delimiters overlap), converts the content by given converter, joins the output with the same delimiter and
    /// writes it to the output.
    /// </summary>
    /// <param name="delimiter">Delimiter (can be more than one byte long).</param>
    /// <param name="inputFilePath">Input file path. If null, stdin is used.</param>
    /// <param name="outputFilePath">Output file path. If null, stdout is used.</param>
    public void ProcessInput(string? delimiter = null, string? inputFilePath = null, string? outputFilePath = null)
    {
        // Setup input and output streams (file or stdin/stdout)
        using var inputStream = inputFilePath is null
            ? Console.OpenStandardInput()
            : new FileStream(inputFilePath, FileMode.Open);

        using var outputStream = outputFilePath is null
            ? Console.OpenStandardOutput()
            : new FileStream(outputFilePath, FileMode.Truncate);

        switch (delimiter)
        {
            case null:
                // Delimiter is null -> don't take delimiter into account, process whole file
                ProcessWithoutDelimiter(inputStream, outputStream);
                break;

            case "":
                // Delimiter is empty string -> convert after every char
                ProcessEmptyDelimiter(inputStream, outputStream);
                break;

            default:
                // Delimiter is at least one char long -> use Knuth–Morris–Pratt algorithm
                var byteDelimiter = Encoding.UTF8.GetBytes(delimiter);
                ProcessWithKmp(byteDelimiter, inputStream, outputStream);
                break;
        }
    }
}