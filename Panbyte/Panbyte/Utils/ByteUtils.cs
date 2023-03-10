using Panbyte.Formats.Enums;

namespace Panbyte.Utils;

public static class ByteUtils
{
    public static string ConvertToString(byte b, ArrayFormat format) =>
        format switch
        {
            ArrayFormat.Hex => "0x" + b.ToString("x2"),
            ArrayFormat.Decimal => Convert.ToString(b, 10),
            ArrayFormat.Binary => "0b" + Convert.ToString(b, 2),
            ArrayFormat.Char => ConvertToChar(b),
            _ => throw new ArgumentException("Invalid byte format"),
        };

    private static string ConvertToChar(byte b)
    {
        if (Char.IsControl(Convert.ToChar(b)))
        {
            return $"'\\x{b.ToString().PadLeft(2, '0')}'";
        }

        return $"'{Convert.ToChar(b)}'";
    }
}