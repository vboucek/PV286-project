using Panbyte.Formats.Enums;

namespace Panbyte.Formats;

public class Int : Format
{
    public Endianness Endianness { get; private set; } = Endianness.BigEndian;

    public Int()
    {
    }

    public Int(Endianness endianness)
    {
        Endianness = endianness;
    }

    public override void ParseInputFormatOption(string option)
    {
        switch (option)
        {
            case "big":
                Endianness = Endianness.BigEndian;
                break;

            case "little":
                Endianness = Endianness.LittleEndian;
                break;

            default:
                base.ParseInputFormatOption(option);
                break;
        }
    }
    
    public override void ParseOutputFormatOption(string option)
    {
        switch (option)
        {
            case "big":
                Endianness = Endianness.BigEndian;
                break;

            case "little":
                Endianness = Endianness.LittleEndian;
                break;

            default:
                base.ParseOutputFormatOption(option);
                break;
        }
    }
}