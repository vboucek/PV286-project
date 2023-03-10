using System.Numerics;
using System.Text;
using Panbyte.Formats;
using Panbyte.Formats.Enums;
using Panbyte.Utils;

namespace Panbyte.Converters;


public class BytesConverter : IConverter
{
    public IFormat InputFormat { get; }
    
    public BytesConverter(Bytes format)
    {
        InputFormat = format;
    }

    public string ConvertTo(string value, IFormat outputFormat)
    {
        var bytes = Encoding.UTF8.GetBytes(value);

        if (bytes.Length == 0 && outputFormat.GetType() != typeof(ByteArray))
        {
            return "";
        }
        
        return outputFormat switch
        {
            Bytes => value,
            Hex => Convert.ToHexString(bytes).ToLower(),
            Bits => string.Join("", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))),
            Int intFormat => new BigInteger(bytes, true, intFormat.Endianness == Endianness.BigEndian).ToString(),
            ByteArray arrayFormat => ByteArrayUtils.ConvertToString(bytes, arrayFormat),
            _ => throw new ArgumentException("Invalid format for conversion")
        };
    }
}
