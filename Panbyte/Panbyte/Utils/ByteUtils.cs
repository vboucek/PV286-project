using Panbyte.Formats.Enums;

namespace Panbyte.Utils;

/// <summary>
/// Utility for converting a byte in a specified textual format.
/// </summary>
public static class ByteUtils
{
    /// <summary>
    /// Converts a byte in specified format.
    /// </summary>
    /// <param name="b">Byte for conversion.</param>
    /// <param name="format">Output format.</param>
    /// <returns>Converted byte.</returns>
    /// <exception cref="ArgumentException">when unknown ArrayFormat is given.</exception>
    public static string ConvertToString(byte b, ArrayFormat format) =>
        format switch
        {
            ArrayFormat.Hex => "0x" + b.ToString("x2"),
            ArrayFormat.Decimal => Convert.ToString(b, 10),
            ArrayFormat.Binary => "0b" + Convert.ToString(b, 2),
            ArrayFormat.Char => ConvertByteToChar(b),
            _ => throw new ArgumentException("Invalid byte format"),
        };

    /// <summary>
    /// Converts given byte to char. Unprintable characters are represented with its hexadecimal value.
    /// </summary>
    /// <param name="b">Byte for conversion.</param>
    /// <returns>Converted character.</returns>
    private static string ConvertByteToChar(byte b)
    {
        if (Char.IsControl(Convert.ToChar(b)))
        {
            return $"'\\x{b.ToString().PadLeft(2, '0')}'";
        }

        return $"'{Convert.ToChar(b)}'";
    }
}