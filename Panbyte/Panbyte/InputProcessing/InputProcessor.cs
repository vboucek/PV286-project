using System.Text;
using Panbyte.Converters;
using Panbyte.Formats;

namespace Panbyte.InputProcessing;

public class InputProcessor
{
    private readonly IConverter _converter;
    private readonly Format _outputFormat;

    public InputProcessor(IConverter converter, Format outputFormat)
    {
        _converter = converter;
        _outputFormat = outputFormat;
    }

    private static int[] KmpTable(string delimiter)
    {
        var table = new int[delimiter.Length];
        table[0] = -1;

        var candidate = 0;

        for (int i = 1; i < delimiter.Length; i++)
        {
            if (delimiter[i] == delimiter[candidate])
            {
                table[i] = table[candidate];
            }
            else
            {
                table[i] = candidate;
                while (candidate >= 0 && delimiter[i] != delimiter[candidate])
                {
                    candidate = table[candidate];
                }
            }

            candidate++;
        }

        return table;
    }

    private void ProcessWithKmp(string delimiter, TextReader reader, TextWriter writer)
    {
        StringBuilder buffer = new(); // Current buffer read from stream

        var bufferIndex = 0; // Position in buffer
        var delimiterIndex = 0; // Position in delimiter
        var table = KmpTable(delimiter);

        while (reader.Peek() > 0)
        {
            // Append the next character
            buffer.Append((char)reader.Read());

            if (delimiter[delimiterIndex] == buffer[bufferIndex])
            {
                bufferIndex++;
                delimiterIndex++;

                if (delimiterIndex == delimiter.Length)
                {
                    // found delimiter
                    var position = bufferIndex - delimiterIndex;

                    writer.Write(_converter.ConvertTo(buffer.Remove(position, buffer.Length - position).ToString(),
                        _outputFormat));
                    writer.Write(delimiter);

                    // reset buffer
                    bufferIndex = 0;
                    delimiterIndex = 0;
                    buffer.Clear();
                }
            }
            else
            {
                delimiterIndex = table[delimiterIndex];

                if (delimiterIndex < 0)
                {
                    bufferIndex++;
                    delimiterIndex++;
                }
            }
        }

        // Write the last item (content after last occurrence of delimiter)
        writer.WriteLine(_converter.ConvertTo(buffer.ToString(), _outputFormat));
    }

    private void ProcessEmptyDelimiter(TextReader reader, TextWriter writer)
    {
        while (reader.Peek() > 0)
        {
            writer.Write(_converter.ConvertTo(((char)reader.Read()).ToString(), _outputFormat));
        }

        writer.WriteLine();
    }

    private void ProcessWithoutDelimiter(TextReader reader, TextWriter writer)
    {
        writer.WriteLine(_converter.ConvertTo(reader.ReadToEnd(), _outputFormat));
    }


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