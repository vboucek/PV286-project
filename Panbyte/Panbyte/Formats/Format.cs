namespace Panbyte.Formats;

public abstract class Format
{
    public virtual void ParseInputFormatOption(string option) =>
        throw new ArgumentException($"Invalid input format option '{option}'.");

    public virtual void ParseOutputFormatOption(string option) =>
        throw new ArgumentException($"Invalid output format option '{option}'.");

    public string? DefaultDelimiter { get; protected set; } = Environment.NewLine;
}