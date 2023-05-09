namespace Panbyte.Formats;

public interface IFormatModule
{
    public string Name { get; }
    public string Description { get; }
    public Format GetNewFormat();
}