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
   
    /// <summary>
    /// Converts an array of bytes 
    /// </summary>
    /// <param name="value">an array of bytes</param>
    /// <param name="outputFormat">the output format </param>
    /// <returns>bytes of converted output</returns>
    public byte[] ConvertTo(byte[] value, Format outputFormat)
    {
        if (value.Length == 0 && typeof(ByteArray) != outputFormat.GetType())
            return Array.Empty<byte>();

        return BaseConvertTo(value, outputFormat);
    }
}
