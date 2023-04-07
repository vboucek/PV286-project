namespace Panbyte.Converters.AuxiliaryObjects;

public class Bytes : ArrayContentItem
{
    private byte[] Content { get; }
    
    public Bytes(byte[] content)
    {
        Content = content;
    }
}