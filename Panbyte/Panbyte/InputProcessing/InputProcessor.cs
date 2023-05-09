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
    /// Reads the minimum number of bytes from the binary reader. Stdin cannot read stream on byte at the time. 
    /// </summary>
    /// <param name="reader">Binary reader.</param>
    /// <returns>Byte array read from the binary reader.</returns>
    /// <exception cref="ArgumentException">when binary cannot read.</exception>
    private byte[] GetMinimumBytes(BinaryReader reader)
    {
        var bytesToRead = 1;
        while (true)
        {
            try
            {
                byte[] result = reader.ReadBytes(bytesToRead);
                return result;
            }
            catch (ArgumentException)
            {
                if (bytesToRead > 100)
                    throw new ArgumentException("Decoding error, invalid bytes at input");
                bytesToRead += 1;
            }
        }
    }

    /// <summary>
    /// Implementation of Knuth Morris Pratt algorithm for finding a delimiter in a byte stream.
    /// </summary>
    /// <param name="delimiter">Delimiter for splitting the input.</param>
    /// <param name="inputReader">Input binary reader.</param>
    /// <param name="outputWriter">Output binary writer.</param>
    private void ProcessWithKmp(byte[] delimiter, BinaryReader inputReader, BinaryWriter outputWriter)
    {
        List<byte> buffer = new(); // Current buffer read from the stream

        var bufferIndex = 0; // Position in the buffer
        var delimiterIndex = 0; // Position in the delimiter
        var table = KmpTable(delimiter); // Build a partial match table
        var currentBytes = GetMinimumBytes(inputReader);

        if (currentBytes.Length == 0)
        {
            // Nothing to read in the stream, exit whole function
            return;
        }

        buffer.AddRange(currentBytes);

        while (bufferIndex < buffer.Count)
        {
            if (delimiter[delimiterIndex] == buffer[bufferIndex])
            {
                delimiterIndex++;
                bufferIndex++;

                if (delimiterIndex == delimiter.Length)
                {
                    // Found delimiter
                    var delimiterStart = bufferIndex - delimiterIndex;

                    // Convert the buffer and append the delimiter
                    outputWriter.Write(
                        _converter.ConvertTo(buffer.GetRange(0, delimiterStart).ToArray(), _outputFormat));
                    outputWriter.Write(delimiter);

                    // Reset the buffer and pointers, start over
                    buffer.RemoveRange(0, delimiterStart + delimiter.Length);
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

            currentBytes = GetMinimumBytes(inputReader);
            buffer.AddRange(currentBytes);
        }

        // Write the last item (content after last occurrence of the delimiter)
        outputWriter.Write(_converter.ConvertTo(buffer.ToArray(), _outputFormat));
    }

    /// <summary>
    /// Processes the input with empty delimiter (convert every byte independently).
    /// </summary>
    /// <param name="inputReader">Input binary reader.</param>
    /// <param name="outputWriter">Output binary reader.</param>
    private void ProcessEmptyDelimiter(BinaryReader inputReader, BinaryWriter outputWriter)
    {
        while (true)
        {
            var currentBytes = GetMinimumBytes(inputReader);

            if (currentBytes.Length == 0)
            {
                break;
            }

            outputWriter.Write(_converter.ConvertTo(currentBytes, _outputFormat));
        }
    }

    /// <summary>
    /// Processes the input without considering any delimiter.
    /// </summary>
    /// <param name="inputReader">Input binary reader.</param>
    /// <param name="outputReader">Output binary reader.</param>
    private void ProcessWithoutDelimiter(BinaryReader inputReader, BinaryWriter outputReader)
    {
        List<byte> buffer = new();

        while (true)
        {
            var currentBytes = GetMinimumBytes(inputReader);

            if (currentBytes.Length == 0)
            {
                break;
            }

            buffer.AddRange(currentBytes);
        }

        outputReader.Write(_converter.ConvertTo(buffer.ToArray(), _outputFormat));
    }


    /// <summary>
    /// Reads the program byte input, splits it with given delimiter (first delimiter occurrence is taken into account,
    /// if delimiters overlap), converts the content by given converter, joins the output with the same delimiter and
    /// writes it to the output.
    /// </summary>
    /// <param name="delimiter">Delimiter (can be more than one byte long).</param>
    /// <param name="inputFilePath">Input file path. If null, stdin is used.</param>
    /// <param name="outputFilePath">Output file path. If null, stdout is used.</param>
    public void ProcessInput(byte[]? delimiter = null, string? inputFilePath = null, string? outputFilePath = null)
    {
        // Setup input and output streams (file or stdin/stdout)
        using var inputReader = inputFilePath is null
            ? new BinaryReader(Console.OpenStandardInput())
            : new BinaryReader(new FileStream(inputFilePath, FileMode.Open));

        using var outputWriter = outputFilePath is null
            ? new BinaryWriter(Console.OpenStandardOutput())
            : new BinaryWriter(new FileStream(outputFilePath, FileMode.Create));

        if (delimiter == null)
            // Delimiter is null -> don't take delimiter into account, process whole file
            ProcessWithoutDelimiter(inputReader, outputWriter);
        else if (delimiter.Length == 0)
            // Delimiter is empty -> process byte by byte
            ProcessEmptyDelimiter(inputReader, outputWriter);
        else
            // Delimiter is at least one char long -> use Knuth–Morris–Pratt algorithm
            ProcessWithKmp(delimiter, inputReader, outputWriter);
    }
}
