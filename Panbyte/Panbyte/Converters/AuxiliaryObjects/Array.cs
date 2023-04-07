namespace Panbyte.Converters.AuxiliaryObjects;

public class Array : ArrayContentItem
{
    private List<ArrayContentItem> Content { get; }

    public Array(List<ArrayContentItem> content)
    {
        Content = content;
    }
}