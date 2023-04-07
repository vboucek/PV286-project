namespace Panbyte.Formats;

public class FormatModule<T> : IFormatModule where T : Format, new()
{
    public string Name { get; }
    public string Description { get; }

    public FormatModule(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public Format GetNewFormat()
    {
        return new T();
    }

    public override string ToString()
    {
        return $"{Name}\t\t\t\t\t{Description}";
    }
}