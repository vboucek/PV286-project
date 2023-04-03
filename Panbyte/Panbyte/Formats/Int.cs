using Panbyte.Formats.Enums;

namespace Panbyte.Formats;

/// <summary>
/// Integer format - unsigned integer representation of underlying bytes.
/// </summary>
public class Int : IFormat
{
    public Endianness Endianness { get; set; } = Endianness.BigEndian;

    public Int() { }

    public Int(Endianness endianness)
    {
        Endianness = endianness;
    }

    public override string ToString()
    {
        return "int";
    }
}