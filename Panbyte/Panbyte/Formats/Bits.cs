using Panbyte.Formats.Enums;

namespace Panbyte.Formats;

public class Bits : IFormat
{
    public BitPadding BitPadding { get; set; } = BitPadding.Left;

    public Bits() { }

    public Bits(BitPadding bitPadding)
    {
        BitPadding = bitPadding;
    }
    
    public override string ToString()
    {
        return "bits";
    }
}
