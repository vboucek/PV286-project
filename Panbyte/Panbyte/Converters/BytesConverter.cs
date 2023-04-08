using System.Text;
using Panbyte.Formats;

namespace Panbyte.Converters;

/// <summary>
/// Converter for converting from raw bytes format.
/// </summary>
public class BytesConverter : ByteSequenceConverterBase, IConverter
{
    public Format InputFormat { get; }
    
    public BytesConverter(Bytes format)
    {
        InputFormat = format;
    }
    
    static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }
    
    public byte[] ConvertTo(byte[] value, Format outputFormat)
    {
        if (value.Length == 0 && typeof(ByteArray) != outputFormat.GetType())
            return Array.Empty<byte>();

        return BaseConvertTo(value, outputFormat);
    }
}
