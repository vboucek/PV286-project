using Panbyte.Formats.Enums;

namespace Panbyte.Formats;


/// <summary>
/// Byte array format - byte array as represented in programming languages.
/// </summary>
public class ByteArray : Format
{
    public ArrayFormat ArrayFormat { get; set; } = ArrayFormat.Hex;
    public Brackets Brackets { get; set; } = Brackets.Curly;

    public ByteArray()
    {
    }

    public ByteArray(ArrayFormat arrayFormat, Brackets brackets)
    {
        ArrayFormat = arrayFormat;
        Brackets = brackets;
    }

    public override void ParseOutputFormatOption(string option)
    {
        switch (option)
        {
            case "0x":
                ArrayFormat = ArrayFormat.Hex;
                break;

            case "0":
                ArrayFormat = ArrayFormat.Decimal;
                break;

            case "a":
                ArrayFormat = ArrayFormat.Char;
                break;

            case "0b":
                ArrayFormat = ArrayFormat.Binary;
                break;

            case "{" or "}" or "{}":
                Brackets = Brackets.Curly;
                break;

            case "(" or ")" or "()":
                Brackets = Brackets.Regular;
                break;

            case "[" or "]" or "[]":
                Brackets = Brackets.Square;
                break;

            default:
                base.ParseOutputFormatOption(option);
                break;
        }
    }
}
