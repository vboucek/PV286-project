using System.Numerics;
using Panbyte.Formats;
using Panbyte.Formats.Enums;
using Panbyte.Utils;

namespace Panbyte.Converters;

public class IntConverter: ByteSequenceConverterBase, IConverter
{
    public IFormat InputFormat { get; }

    public IntConverter(Int format)
    {
        InputFormat = format;
    }
    
    public string ConvertTo(string value, IFormat outputFormat)
    {
 
        BigInteger bigInteger;
        var success = BigInteger.TryParse(value, out bigInteger);

        if (!success)
        {
            throw new ArgumentException("Invalid value for conversion");
        }
        
        var inputEndianness = ((Int) InputFormat).Endianness;
        var bytes = bigInteger.ToByteArray(true, inputEndianness == Endianness.BigEndian);

        return BaseConvertTo(bytes, outputFormat);
    }
}