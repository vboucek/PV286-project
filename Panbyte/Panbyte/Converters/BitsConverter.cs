using System.Numerics;
using System.Text;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Panbyte.Converters;

/// <summary>
/// Converter for converting from bits format.
/// </summary>
public class BitsConverter : ByteSequenceConverterBase, IConverter
{
    public Format InputFormat { get; }

    public BitsConverter(Bits format)
    {
        InputFormat = format;
    }

    private BigInteger BinaryStringToBigInteger(string bits)
    {
        BigInteger bigInt = BigInteger.Zero;

        foreach (var bit in bits)
        {
            if (bit != '0' && bit != '1')
            {
                throw new FormatException("Input string contains invalid characters");
            }

            bigInt <<= 1;
            if (bit == '1')
            {
                bigInt |= BigInteger.One;
            }
        }

        return bigInt;
    }

    private byte[] PadAndConvert(string value)
    {
        var stripped = string.Join("", value.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

        var padding = ((Bits)InputFormat).BitPadding;
        var paddingWidth = (stripped.Length % 8 == 0) ? 0 : (8 - stripped.Length % 8);

        var bits = padding switch
        {
            BitPadding.Left => stripped.PadLeft(stripped.Length + paddingWidth, '0'),
            BitPadding.Right => stripped.PadRight(stripped.Length + paddingWidth, '0'),
            _ => throw new FormatException("Invalid bit padding type"),
        };

        return BinaryStringToBigInteger(bits).ToByteArray(true, true);
    }

    public byte[] ConvertTo(byte[] value, Format outputFormat)
    {
        if (value.Length == 0 && typeof(ByteArray) != outputFormat.GetType())
            return Array.Empty<byte>();

        if (value.Length == 0)
            return BaseConvertTo(value, outputFormat);

        var str = Encoding.ASCII.GetString(value);
        var bytes = PadAndConvert(str);
        return BaseConvertTo(bytes, outputFormat);
        
    }
}