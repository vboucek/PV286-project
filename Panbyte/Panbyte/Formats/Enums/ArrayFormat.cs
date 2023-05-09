namespace Panbyte.Formats.Enums;

/// <summary>
/// Byte Array option - element format.
/// </summary>
public enum ArrayFormat
{
    Hex, // hexadecimal number (e.g., 0xff; default)
    Decimal, // decimal number (e.g., 255)
    Char, // character (e.g., 'a', '\x00')
    Binary // binary number (e.g., 0b11111111)
}
