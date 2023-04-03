using Panbyte.Formats.Enums;

namespace Panbyte.Formats;


/// <summary>
/// Byte array format - byte array as represented in programming languages.
/// </summary>
public class ByteArray : IFormat
{
    public ArrayFormat ArrayFormat { get; set; } = ArrayFormat.Hex;
    public Brackets Brackets { get; set; } = Brackets.Curly;

    public ByteArray() { }

    public ByteArray(ArrayFormat arrayFormat, Brackets brackets)
    {
        ArrayFormat = arrayFormat;
        Brackets = brackets;
    }
    
    public override string ToString()
    {
        return "array";
    }
}
