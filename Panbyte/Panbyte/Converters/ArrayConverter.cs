using System.Text;
using System.Text.RegularExpressions;
using Panbyte.Converters.AuxiliaryObjects;
using Panbyte.Formats;
using Panbyte.Utils;
using Array = System.Array;

namespace Panbyte.Converters;

public class ArrayConverter : ByteSequenceConverterBase, IConverter
{
    public ArrayConverter(ByteArray inputFormat)
    {
        InputFormat = inputFormat;
    }

    public Format InputFormat { get; }

    private byte[] Input { get; set; }
    private int LastIndex { get; set; }

    private static readonly HashSet<byte> OpeningBrackets =
        new() { Convert.ToByte('{'), Convert.ToByte('['), Convert.ToByte('(') };

    private static readonly HashSet<byte> ClosingBrackets =
        new() { Convert.ToByte('}'), Convert.ToByte(']'), Convert.ToByte(')') };

    private static bool TryHex(string item, ref byte result)
    {
        var rgxHex = new Regex(@"^0x[\da-f][\da-f]$");
        if (!rgxHex.IsMatch(item)) return false;
        var resultArrayBytes = Convert.FromHexString(item.Substring(2, 2));
        result = resultArrayBytes[0]; // item will never be more than 1 byte
        return true;
    }

    private static bool TryDecimal(string item, ref byte result)
    {
        if (!Decimal.TryParse(item, out var resDecimal) || resDecimal < 0) return false;
        if (resDecimal >= 256) throw new FormatException("Invalid array format");
        result = Decimal.ToByte(resDecimal);
        return true;
    }

    private static bool TryCharacter(string item, ref byte result)
    {
        var rgxHex = new Regex(@"'\\x[\da-f][\da-f]'");
        var rgxChar = new Regex(@"^'.'$");
        
        if (rgxHex.IsMatch(item))
        {
            var resultArrayBytes = Convert.FromHexString(item.Substring(3, 2));
            result = resultArrayBytes[0];
            return true;
        }
        
        if (rgxChar.IsMatch(item))
        {
            var ord = (int) item[1];
            if (ord is < 0 or > 255) throw new FormatException("Invalid array format");
            result = Convert.ToByte(item[1]);
            return true;
        }

        return false;
    }

    private static bool TryBinary(string item, ref byte result)
    {
        var rgxBinary = new Regex(@"^0b[01]{1,8}$");
        if (!rgxBinary.IsMatch(item)) return false;

        var stripped = item.Substring(2, item.Length - 2);
        var paddingWidth = (stripped.Length % 8 == 0) ? 0 : (8 - stripped.Length % 8);
        var padded = stripped.PadLeft(stripped.Length + paddingWidth, '0');

        result = Convert.ToByte(padded, 2);
        return true;
    }

    private static byte MatchConvertToFormat(string item)
    {
        var result = new byte();
        if (TryHex(item, ref result) || TryDecimal(item, ref result) || TryCharacter(item, ref result) ||
            TryBinary(item, ref result))
        {
            return result;
        }

        throw new FormatException("Item in the array is of wrong format");
    }

    private static byte ValidateConvertStringItem(string item)
    {
        var stripedItemString = item.Trim();
        return MatchConvertToFormat(stripedItemString);
    }

    private static void FinishItem(object item, List<ArrayContentItem> content)
    {
        switch (item)
        {
            case AuxiliaryObjects.Array:
                content.Add((ArrayContentItem)item);
                break;
            case List<byte> byteListItem:
                var stringItem = Encoding.UTF8.GetString(byteListItem.ToArray());
                if (stringItem is null) throw new NullReferenceException("Item is null and should not be.");
                var bytesItem = ValidateConvertStringItem(stringItem);
                content.Add(new AuxiliaryObjects.Byte(bytesItem));
                break;
            default:
                throw new FormatException("Item in the array is neither of type Array or byte list");
        }
    }

    private static void ValidateClosingBracket(byte openingBracket, byte charByte)
    {
        if (openingBracket == Convert.ToByte('\0')) throw new FormatException("Opening bracket is missing");

        var expectedClosingBracket = GetMatchingBracket.GetMatchingClosingBracket(openingBracket);
        if (charByte != expectedClosingBracket)
        {
            throw new FormatException($"Closing bracket {charByte} does NOT match opening bracket");
        }
    }

    private (object item, int currentIndex, int openingBracketsNumber) CreateObject(int currentIndex,
        int openingBracketsNumber, byte openingBracket, bool fstItmInArray)
    {
        object item = new List<byte>();

        var isInsideItem = false;

        List<ArrayContentItem> content = new();

        while (currentIndex <= LastIndex)
        {
            var charByte = Input[currentIndex];

            if (OpeningBrackets.Contains(charByte))
            {
                if (isInsideItem) throw new FormatException("Invalid array format");

                openingBracketsNumber++;
                (item, currentIndex, openingBracketsNumber) =
                    CreateObject(currentIndex + 1, openingBracketsNumber, charByte, true);
                isInsideItem = false;
            }
            else if (charByte == Convert.ToByte(','))
            {
                if (item is List<byte> && ((List<byte>)item).Count == 0 && !fstItmInArray)
                    throw new FormatException("Invalid array format");

                FinishItem(item, content);
                item = new List<byte>();
                currentIndex++;
                isInsideItem = false;
                fstItmInArray = false;
            }
            else if (ClosingBrackets.Contains(charByte))
            {
                if (item is List<byte> && ((List<byte>)item).Count == 0 && !fstItmInArray)
                    throw new FormatException("Invalid array format");

                ValidateClosingBracket(openingBracket, charByte);

                if (item is List<byte> && Encoding.UTF8.GetString(((List<byte>)item).ToArray()).Trim().Length == 0 &&
                    fstItmInArray)
                {
                    return (new AuxiliaryObjects.Array(new List<ArrayContentItem>()), currentIndex + 1,
                        openingBracketsNumber);
                }

                FinishItem(item, content);
                return (new AuxiliaryObjects.Array(content), currentIndex + 1, openingBracketsNumber);
            }
            else if (char.IsWhiteSpace(Convert.ToChar(charByte)) && !isInsideItem)
            {
                currentIndex++;
            }
            else
            {
                if (item is AuxiliaryObjects.Array)
                {
                    throw new FormatException("Invalid array format");
                }

                isInsideItem = true;
                ((List<byte>)item).Add(charByte);
                currentIndex++;
            }
        }

        if (openingBracket != Convert.ToByte('\0')) throw new FormatException("Closing bracket is missing");

        return (item, currentIndex + 1, openingBracketsNumber);
    }

    private void CheckFromToIfNested(int openingBracketsNumber, Format outputFormat)
    {
        if (openingBracketsNumber >= 2 && (InputFormat is not ByteArray || outputFormat is not ByteArray))
        {
            throw new FormatException("Array is nested, however not both formats are of type array");
        }
    }

    private AuxiliaryObjects.Array ValidateParseInput(Format outputFormat)
    {
        LastIndex = Input.Length - 1;

        if (Input.Length == 0)
        {
            throw new FormatException("Input is not an array");
        }

        var (result, _, openingBracketsNumber) = CreateObject(0, 0,
            Convert.ToByte('\0'), false);

        CheckFromToIfNested(openingBracketsNumber, outputFormat);

        return (AuxiliaryObjects.Array)result;
    }

    private byte[] OutputNotByteArray(AuxiliaryObjects.Array input, Format outputFormat)
    {
        var items = new List<byte>();

        foreach (var arrayContItm in input.Content)
        {
            var byteItem = (AuxiliaryObjects.Byte)arrayContItm;
            items.Add(byteItem.Content);
        }

        return BaseConvertTo(items.ToArray(), outputFormat);
    }

    public byte[] ConvertTo(byte[] value, Format outputFormat)
    {
        Input = value;
        var parsedInput = ValidateParseInput(outputFormat);

        if (outputFormat is ByteArray array)
        {
            return parsedInput.ArrayContentToByteArray(array);
        }

        return OutputNotByteArray(parsedInput, outputFormat);
    }
}