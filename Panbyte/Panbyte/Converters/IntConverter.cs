using System.Numerics;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Panbyte.Converters;

/// <summary>
/// Converter for converting from integer format.
/// </summary>
public class IntConverter: ByteSequenceConverterBase, IConverter
{
    public IFormat InputFormat { get; }

    public IntConverter(Int format)
    {
        InputFormat = format;
    }
    
    public string ConvertTo(string value, IFormat outputFormat)
    {
        var success = BigInteger.TryParse(value, out var bigInteger);

        if (!success)
        {
            throw new FormatException("Input string contains invalid characters");
        }
        
        var inputEndianness = ((Int) InputFormat).Endianness;
        var bytes = bigInteger.ToByteArray(true, inputEndianness == Endianness.BigEndian);

        return BaseConvertTo(bytes, outputFormat);
    }
}