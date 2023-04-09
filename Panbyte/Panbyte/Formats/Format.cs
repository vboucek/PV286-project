namespace Panbyte.Formats;
using System.Text;

public abstract class Format
{
    public virtual void ParseInputFormatOption(string option) =>
        throw new ArgumentException($"Invalid input format option '{option}'.");

    public virtual void ParseOutputFormatOption(string option) =>
        throw new ArgumentException($"Invalid output format option '{option}'.");

    public byte[]? DefaultDelimiter { get; protected set; } = Encoding.ASCII.GetBytes(Environment.NewLine);
}