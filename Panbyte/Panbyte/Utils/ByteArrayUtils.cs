using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Panbyte.Utils;

/// <summary>
/// Utility for pretty-printing an array of bytes.
/// </summary>
public static class ByteArrayUtils
{
    /// <summary>
    /// Converts an array of bytes in its textual representation based on specified format.
    /// </summary>
    /// <param name="bytes">Array of bytes for conversion.</param>
    /// <param name="format">Output Byte Array format.</param>
    /// <returns>Given array of bytes converted specified output format.</returns>
    public static byte[] ConvertToBytes(byte[] bytes, ByteArray format)
    {
        var printedArray = String.Join(", ", bytes.Select(b => ByteUtils.ConvertToString(b, format.ArrayFormat)));

        return new byte[] { };
        //return GetOpeningBracket(format.Brackets) + printedArray + GetClosingBracket(format.Brackets);
    }

    public static byte GetOpeningBracket(Brackets bracketsType) =>
        bracketsType switch
        {
            Brackets.Curly => Convert.ToByte('{'),
            Brackets.Regular => Convert.ToByte('('),
            Brackets.Square => Convert.ToByte('['),
            _ => Convert.ToByte('{'),
        };

    public static byte GetClosingBracket(Brackets bracketsType) =>
        bracketsType switch
        {
            Brackets.Curly => Convert.ToByte('}'),
            Brackets.Regular => Convert.ToByte(')'),
            Brackets.Square => Convert.ToByte(']'),
            _ => Convert.ToByte('}'),
        };
}