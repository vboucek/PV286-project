using System.Numerics;
using System.Text;
using Panbyte.Formats;
using Panbyte.Formats.Enums;
using Panbyte.Utils;

namespace Panbyte.Converters;

/// <summary>
/// Converts array of bytes in specified format.
/// </summary>
public abstract class ByteSequenceConverterBase
{

    
    protected byte[] BaseConvertTo(byte[] bytes, Format outputFormat)
    {
        return outputFormat switch
        {
            Bytes => bytes,
            Hex =>  Encoding.ASCII.GetBytes(Convert.ToHexString(bytes).ToLower()),
            Bits => Encoding.ASCII.GetBytes(string.Join("", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')))),
            Int intFormat => Encoding.ASCII.GetBytes(new BigInteger(bytes, true, intFormat.Endianness == Endianness.BigEndian).ToString()),
            ByteArray arrayFormat => Encoding.ASCII.GetBytes(ByteArrayUtils.ConvertToString(bytes, arrayFormat)),
            _ => throw new ArgumentException("Invalid format for conversion")
        };
    }
}
