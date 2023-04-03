using Panbyte.Formats.Enums;

namespace Panbyte.Formats;

public class Bits : Format
{
    public BitPadding BitPadding { get; private set; } = BitPadding.Left;

    public Bits() { }

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
