using Panbyte.Formats.Enums;

namespace Panbyte.Formats;

/// <summary>
/// Bits format - string of 0 and 1 characters, representing a sequence of bits.
/// </summary>
public class Bits : IFormat
{
    public BitPadding BitPadding { get; set; } = BitPadding.Left;

    public Bits()
    {
    }

    public Bits(BitPadding bitPadding)
    {
        BitPadding = bitPadding;
    }

    public override string ToString()
    {
        return "bits";
    }
}