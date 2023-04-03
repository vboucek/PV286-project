using Panbyte.Formats.Enums;

namespace Panbyte.Formats;

/// <summary>
/// Bits format - string of 0 and 1 characters, representing a sequence of bits.
/// </summary>
public class Bits : Format
{
    public BitPadding BitPadding { get; private set; } = BitPadding.Left;

    public Bits()
    {
    }

    public Bits(BitPadding bitPadding)
    {
        BitPadding = bitPadding;
    }

    public override void ParseInputFormatOption(string option)
    {
        switch (option)
        {
            case "left":
                BitPadding = BitPadding.Left;
                break;

            case "right":
                BitPadding = BitPadding.Right;
                break;

            default:
                base.ParseInputFormatOption(option);
                break;
        }
    }
}
