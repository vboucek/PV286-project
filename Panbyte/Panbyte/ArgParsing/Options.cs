using Panbyte.Formats;

namespace Panbyte.ArgParsing;

public class Options
{
    public IFormat InputFormat { get; }
    public IFormat OutputFormat { get; }
    public string? InputFilePath { get; set; }
    public string? OutputFilePath { get; set; }
    public string Delimiter { get; set; } = Environment.NewLine;
    public bool Help { get; set; }

    public Options(IFormat inputFormat, IFormat outputFormat)
    {
        InputFormat = inputFormat;
        OutputFormat = outputFormat;
    }
}