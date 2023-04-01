using System.Text;
using Panbyte.ArgParsing;
using Panbyte.Converters;
using Panbyte.Formats;

namespace Panbyte.InputProcessing;

public class InputProcessor
{
    private readonly IConverter _converter;
    private readonly FullOptions _options;

    public InputProcessor(IConverter converter, FullOptions options)
    {
        _converter = converter;
        _options = options;
    }
    
    private void ProcessLinesInput(TextReader textReader, TextWriter textWriter)
    {
        long lineNum = 0;

        while (textReader.ReadLine() is { } line)
        {
            lineNum++;
            string convertedLine;
            
            try
            {
                convertedLine = _converter.ConvertTo(line, _options.OutputFormat);
            }
            catch (FormatException e)
            {
                throw new FormatException($"Error on line {lineNum}: {e}");
            }

            textWriter.WriteLine(convertedLine);
        }
    }
    
    private void ProcessCharInput(TextReader textReader, TextWriter textWriter)
    {
        StringBuilder builder = new();

        if (_options.Delimiter.Length != 1)
        {
            throw new ArgumentException("Delimiter must be one character long.");
        }

        var delimiter = _options.Delimiter[0];
        
        while (textReader.Peek() > 0)
        {
            var c = (char)textReader.Read();

            // found delimiter and input format is not Bytes => convert
            if (_options.Delimiter is not null && _options.InputFormat is not Bytes && c == delimiter)
            {
                textWriter.Write(_converter.ConvertTo(builder.ToString(), _options.OutputFormat));
                textWriter.Write(_options.Delimiter);
                builder.Clear();
            }
            else
            {
                builder.Append(c);    
            }
        } 
        
        textWriter.WriteLine(_converter.ConvertTo(builder.ToString(), _options.OutputFormat));   
    }
    
    public void ProcessInput()
    {
        using var textReader = _options.InputFilePath is null ? Console.In : new StreamReader(_options.InputFilePath);
        using var textWriter = _options.OutputFilePath is null ? Console.Out : new StreamWriter(_options.OutputFilePath);

        if (_options.Delimiter == Environment.NewLine)
        {
            ProcessLinesInput(textReader, textWriter);
        }
        else
        {
            ProcessCharInput(textReader, textWriter);
        }
    }
}