using System.Text;
using Panbyte.Formats;

namespace Panbyte.Converters;

/// <summary>
/// Converter for converting from raw bytes format.
/// </summary>
public class BytesConverter : ByteSequenceConverterBase, IConverter
{
    public IFormat InputFormat { get; }
    
    public BytesConverter(Bytes format)
    {
        InputFormat = format;
    }

    public string ConvertTo(string value, IFormat outputFormat)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        return BaseConvertTo(bytes, outputFormat);
    }
}
