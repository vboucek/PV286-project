using System.Text;
using System.Text.RegularExpressions;
using Panbyte.Converters.AuxiliaryObjects;
using Panbyte.Formats;
using Panbyte.Utils;
using Array = System.Array;
using Byte = Panbyte.Converters.AuxiliaryObjects.Byte;

namespace Panbyte.Converters;

public class ArrayConverter : IConverter
{
    public Format InputFormat { get; }
    
    private byte[] Input { get; set; }
    private int LastIndex { get; set; }

    private static readonly HashSet<byte> OpeningBrackets = 
        new() {Convert.ToByte('{'), Convert.ToByte('['), Convert.ToByte('(')};
    private static readonly HashSet<byte> ClosingBrackets =
        new() {Convert.ToByte('}'), Convert.ToByte(']'), Convert.ToByte(')')};
    
    private static bool TryHex(string item, ref byte result)
    {
        var rgxHex = new Regex(@"^0x[\da-f][\da-f]$");
        if (!rgxHex.IsMatch(item)) return false;
        var resultArrayBytes = Convert.FromHexString(item.Substring(2, 2));
        result = resultArrayBytes[0];  // item will never be more than 1 byte
        return true;
    }

    private static bool TryDecimal(string item, ref byte result)
    {
        if (!Decimal.TryParse(item, out var resDecimal) || resDecimal < 0) return false;
        result = Decimal.ToByte(resDecimal);
        return true;
    }
    
    private static bool TryCharacter(string item, ref byte result)
    {
        var rgxChar = new Regex(@"^'.'$");
        var rgxHex = new Regex(@"'\\x[\da-f][\da-f]'");
        if (rgxChar.IsMatch(item))
        {
            result = Convert.ToByte(item[1]);
            return true;
        }
        if (rgxHex.IsMatch(item))
        {
            var resultArrayBytes = Convert.FromHexString(item.Substring(2, 2));
            result = resultArrayBytes[0];
            return true;
        }
        return false;
    }

    private static bool TryBinary(string item, ref byte result)
    {
        var rgxBinary = new Regex(@"^0b[01]{1,8}$");
        if (!rgxBinary.IsMatch(item)) return false;
        result = Convert.ToByte(item.Substring(2, 8), 2);
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
            case Array:
                content.Add((ArrayContentItem) item);
                break;
            case List<byte> byteListItem:
                var stringItem = Encoding.UTF8.GetString(byteListItem.ToArray());  // TODO check if all bytes are encode-able?
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
        int openingBracketsNumber, byte openingBracket)
    {
        object item = new List<byte>();

        List<ArrayContentItem> content = new();

        while (currentIndex <= LastIndex)
        {
            var charByte = Input[currentIndex];
            
            if (OpeningBrackets.Contains(charByte))
            {
                openingBracketsNumber += 1;
                (item, currentIndex, openingBracketsNumber) =
                    CreateObject(currentIndex + 1, openingBracketsNumber, charByte);
            }
            else if (charByte == Convert.ToByte(','))
            {
                FinishItem(item, content);
                item = new List<byte>();
                currentIndex += 1;
            }
            else if (ClosingBrackets.Contains(charByte))
            {
                ValidateClosingBracket(openingBracket, charByte);
                FinishItem(item, content);
                
                return (new AuxiliaryObjects.Array(content), currentIndex + 1, openingBracketsNumber);
            }
            // else if (char.IsWhiteSpace(character))
            // {
            //     currentIndex += 1;
            // }
            else
            {
                ((List<byte>) item).Add(charByte);
                currentIndex += 1;
            }
        }

        if (openingBracket == Convert.ToByte('\0')) throw new FormatException("Closing bracket is missing");

        return (item, currentIndex + 1, openingBracketsNumber);
    }

    private void InputIsArray()
    {
        const int firstIndex = 0;
        if (LastIndex < firstIndex || !OpeningBrackets.Contains(Input[firstIndex]))
        {
            throw new FormatException("Input is not a valid array input");
        }
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
        
        InputIsArray();
        
        var (result, _, openingBracketsNumber) = CreateObject(0, 0,
            Convert.ToByte('\0'));

        CheckFromToIfNested(openingBracketsNumber, outputFormat);

        return (AuxiliaryObjects.Array) result;
    }
    
    private static byte[] ConvertContentToByteArray(List<ArrayContentItem> input)
    {
        var byteList = new List<byte>();
        foreach (var inp in input)
        {
            var inpByte = (AuxiliaryObjects.Byte) inp;
            byteList.Add(inpByte.Content);
        }
        return byteList.ToArray();
    }
    
    public byte[] ConvertTo(byte[] value, Format outputFormat)
    {
        Input = value;
        var parsedInput = ValidateParseInput(outputFormat);

        if (outputFormat is ByteArray array)
        {
            return parsedInput.ArrayContentToByteArray(array);
        }

        // TODO extract the following lines of code to the function that will be called
        
        var byteArray = (ByteArray)outputFormat;
        var resultList = new List<byte>() { ByteArrayUtils.GetOpeningBracket(byteArray.Brackets)};

        var items = new List<byte>();
        foreach (var arrayContItm in parsedInput.Content)
        {
            var byteItem = (AuxiliaryObjects.Byte) arrayContItm;
            items.Add(byteItem.Content);
        }
        
        // resultList.AddRange(BaseConvertTo(items.ToArray(), outputFormat));
        resultList.Add(ByteArrayUtils.GetClosingBracket(byteArray.Brackets));
    
        return resultList.ToArray();
    }
}