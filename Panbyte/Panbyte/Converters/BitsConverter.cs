using System.Numerics;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Panbyte.Converters;

public class BitsConverter : ByteSequenceConverterBase, IConverter
{
    public IFormat InputFormat { get; }

    public BitsConverter(Bits format)
    {
        InputFormat = format;
    }

    private BigInteger binaryStringToBigInteger(string bits)
    {
        BigInteger bigInt = BigInteger.Zero;

        foreach (var bit in bits)
        {
            if (bit != '0' && bit != '1')
            {
                throw new ArgumentException("Input string contains invalid characters");
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
            BitPadding.Right => stripped.PadRight(stripped.Length + paddingWidth, '0')
        };
        
        return binaryStringToBigInteger(bits).ToByteArray(true, true);
    }

    public string ConvertTo(string value, IFormat outputFormat)
    {
        var bytes = PadAndConvert(value);
        return BaseConvertTo(bytes, outputFormat);
    }
}