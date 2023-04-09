using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Panbyte.Converters;

/// <summary>
/// Converter for converting from integer format.
/// </summary>
public class IntConverter: ByteSequenceConverterBase, IConverter
{
    public Format InputFormat { get; }

    public IntConverter(Int format)
    {
        InputFormat = format;
    }
   
    /// <summary>
    /// Converts an array of bytes interpreted as unsigned integer in ASCII
    /// </summary>
    /// <param name="value">bytes interpreted as unsigned integer</param>
    /// <param name="outputFormat">the output format</param>
    /// <returns>bytes of the converted result</returns>
    /// <exception cref="FormatException">exception thrown in case input bytes cannot by interpreted as unsigned integer</exception>
    public byte[] ConvertTo(byte[] value, Format outputFormat)
    {
        if (value.Length == 0 && typeof(ByteArray) != outputFormat.GetType())
            return Array.Empty<byte>();

        if (value.Length == 0)
            return BaseConvertTo(value, outputFormat);

        var success = BigInteger.TryParse(Encoding.ASCII.GetString(value), out var bigInteger);

        if (!success)
        {
            throw new FormatException("Input string contains invalid characters");
        }
            
        var inputEndianness = ((Int) InputFormat).Endianness;
        var bytes = bigInteger.ToByteArray(true, inputEndianness == Endianness.BigEndian);

        return BaseConvertTo(bytes, outputFormat);
        
    }
}