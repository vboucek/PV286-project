namespace Panbyte.Converters.AuxiliaryObjects;

public class Byte : ArrayContentItem
{
    public byte Content { get; }
    
    public Byte(byte content)
    {
        Content = content;
    }
}