namespace Panbyte.Converters.AuxiliaryObjects;

public class Array : ArrayItem
{
    public List<ArrayItem> Content { get; }

    public Array(List<ArrayItem> content)
    {
        Content = content;
    }
}