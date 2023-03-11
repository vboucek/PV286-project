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

    private byte[] PadAndConvert(string value)
    {
        var stripped = string.Join("", value.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

        var padding = ((Bits)InputFormat).BitPadding;

        var bits = padding switch
        {
            BitPadding.Left => stripped.PadLeft(8 - stripped.Length % 8, '0'),
            BitPadding.Right => stripped.PadRight(8 - stripped.Length % 8, '0')
        };
        
        BigInteger bigInt = BigInteger.Zero;
        foreach (var bit in bits)
        {
            switch (bit)
            {
                case '0': 
                    bigInt <<= 1;
                    break;
                case '1':
                    bigInt |= 1;
                    break;
                default:
                    throw new ArgumentException("Invalid format of input value");
            }
        }

        return bigInt.ToByteArray(true, true);;
    }

    public string ConvertTo(string value, IFormat outputFormat)
    {
        var bytes = PadAndConvert(value);
        return BaseConvertTo(bytes, outputFormat);
    }
}