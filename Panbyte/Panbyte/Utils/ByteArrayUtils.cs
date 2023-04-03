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

        return GetOpeningBracket(format.Brackets) + printedArray + GetClosingBracket(format.Brackets);
    }

    private static string GetOpeningBracket(Brackets bracketsType) =>
        bracketsType switch
        {
            Brackets.Curly => "{",
            Brackets.Regular => "(",
            Brackets.Square => "[",
            _ => "(",
        };

    private static string GetClosingBracket(Brackets bracketsType) =>
        bracketsType switch
        {
            Brackets.Curly => "}",
            Brackets.Regular => ")",
            Brackets.Square => "]",
            _ => ")",
        };
}