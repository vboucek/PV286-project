using System.Text;
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
    public static string ConvertToString(byte[] bytes, ByteArray format)
    {
        var printedArray = String.Join(", ", bytes.Select(b => ByteUtils.ConvertToString(b, format.ArrayFormat)));

        return GetOpeningBracketString(format.Brackets) + printedArray + GetClosingBracketString(format.Brackets);
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
    
    public static string GetOpeningBracketString(Brackets bracketsType) =>
        bracketsType switch
        {
            Brackets.Curly => "{",
            Brackets.Regular => "(",
            Brackets.Square => "[",
            _ => "{",
        };

    public static string GetClosingBracketString(Brackets bracketsType) =>
        bracketsType switch
        {
            Brackets.Curly => "}",
            Brackets.Regular => ")",
            Brackets.Square => "]",
            _ => "}",
        };
}