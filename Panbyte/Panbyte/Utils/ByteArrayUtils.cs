using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Panbyte.Utils;

public static class ByteArrayUtils
{
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