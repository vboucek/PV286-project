namespace Panbyte.Converters.AuxiliaryObjects;

public class Byte : ArrayContentItem
{
    private byte Content { get; }
    
    public Byte(byte content)
    {
        Content = content;
    }
}