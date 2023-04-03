using System.Numerics;
using Panbyte.Formats;
using Panbyte.Formats.Enums;
using Panbyte.Utils;

namespace Panbyte.Converters;

public abstract class ByteSequenceConverterBase
{
    protected string BaseConvertTo(byte[] bytes, Format outputFormat)
    {
        if (bytes.Length == 0 && outputFormat.GetType() != typeof(ByteArray))
        {
            return "";
        }

        return outputFormat switch
        {
            Bytes => new string(bytes.Select(x => (char)x).ToArray()),
            Hex => Convert.ToHexString(bytes).ToLower(),
            Bits => string.Join("", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))),
            Int intFormat => new BigInteger(bytes, true, intFormat.Endianness == Endianness.BigEndian).ToString(),
            ByteArray arrayFormat => ByteArrayUtils.ConvertToString(bytes, arrayFormat),
            _ => throw new ArgumentException("Invalid format for conversion")
        };
    }
}