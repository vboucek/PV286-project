using Panbyte.Formats;

namespace Panbyte.ArgParsing;

/// <summary>
/// Full options required for running Panbyte console app.
/// </summary>
public class FullOptions : IOptions
{
    public IFormat InputFormat { get; }
    public IFormat OutputFormat { get; }
    public string? InputFilePath { get; set; }
    public string? OutputFilePath { get; set; }
    public string Delimiter { get; set; } = Environment.NewLine;
    public bool Help { get; }

    public FullOptions(IFormat inputFormat, IFormat outputFormat, bool help)
    {
        InputFormat = inputFormat;
        OutputFormat = outputFormat;
        Help = help;
    }
}