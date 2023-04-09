namespace Panbyte.Converters.AuxiliaryObjects;

/// <summary>
/// Item representing bytes in an attribute Content in the object of an instance of class AuxiliaryObjects.
/// </summary>
public class Byte : ArrayContentItem
{
    public byte Content { get; }
    
    public Byte(byte content)
    {
        Content = content;
    }
}