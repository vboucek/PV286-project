using Panbyte.Formats;

namespace Panbyte.OptionsParsing;

/// <summary>
/// Panbyte options parsed from CLI.
/// </summary>
public class PanbyteOptions
{
    public Format? InputFormat { get; set; }
    public Format? OutputFormat { get; set; }
    public string? InputFilePath { get; set; }
    public string? OutputFilePath { get; set; }
    public string? Delimiter { get; set; }
    public bool Help { get; set; }
}