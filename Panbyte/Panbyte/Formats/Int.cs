using Panbyte.Formats.Enums;

namespace Panbyte.Formats;

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