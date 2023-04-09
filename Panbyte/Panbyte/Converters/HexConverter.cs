using System.Text;
using Panbyte.Formats;

namespace Panbyte.Converters;

/// <summary>
/// Converter for converting from hexadecimal format.
/// </summary>
public class HexConverter : ByteSequenceConverterBase, IConverter
{
    public Format InputFormat { get; }

    public HexConverter(Hex format)
    {
        InputFormat = format;
    }


    /// <summary>
    /// Converts an array of bytes interpreted as hexadecimal string in ASCII
    /// </summary>
    /// <param name="value">bytes interpreted as hexadecimal string</param>
    /// <param name="outputFormat">the output folmat</param>
    /// <returns>bytes of the converted results</returns>
    public byte[] ConvertTo(byte[] value, Format outputFormat)
    {
        if (value.Length == 0 && typeof(ByteArray) != outputFormat.GetType())
            return Array.Empty<byte>();

        if (value.Length == 0)
            return BaseConvertTo(value, outputFormat);

        var str = Encoding.ASCII.GetString(value);
        var bytes = Convert.FromHexString(
                    string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)));
        return BaseConvertTo(bytes, outputFormat);
    }
}