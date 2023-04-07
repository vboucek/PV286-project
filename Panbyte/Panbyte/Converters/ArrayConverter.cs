using System.Globalization;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using Panbyte.Converters.AuxiliaryObjects;
using Panbyte.Formats;
using Panbyte.Utils;
using Array = System.Array;
using Byte = System.Byte;

namespace Panbyte.Converters;

public class ArrayConverter : IConverter
{
    public IFormat InputFormat { get; }
    
    private string _input { get; set; }
    private int _lastIndex { get; set; }

    private const string OpeningBrackets = "{[(";
    private const string ClosingBrackets = "}])";

    private static bool TryHex(string item, ref byte[] result)
    {
        var rgxHex = new Regex(@"^0x[\da-f][\da-f]$");
        if (!rgxHex.IsMatch(item)) return false;
        result = Convert.FromHexString(item.Substring(2, 2));
        return true;
    }

    private static bool TryDecimal(string item, ref byte[] result)
    {
        decimal resDecimal;
        if (!Decimal.TryParse(item, out resDecimal) || resDecimal < 0) return false;
        result = new[] {Decimal.ToByte(resDecimal)};
        return true;
    }
    
    private static bool TryCharacter(string item, ref byte[] result)
    {
        var rgxChar = new Regex(@"^'.'$");
        var rgxHex = new Regex(@"'\\x[\da-f][\da-f]'");
        if (rgxChar.IsMatch(item))
        {
            result = new[] { Convert.ToByte(item[1]) };
            return true;
        }
        if (rgxHex.IsMatch(item))
        {
            result = Convert.FromHexString(item.Substring(2, 2));
            return true;
        }
        return false;
    }

    private static bool TryBinary(string item, ref byte[] result)
    {
        var rgxBinary = new Regex(@"^0b[01]{1,8}$");
        if (!rgxBinary.IsMatch(item)) return false;
        result = new[] {Convert.ToByte(item.Substring(2, 8), 2)};
        return true;
    }
    
    private static byte[] MatchConvertToFormat(string item)
    {
        var result = Array.Empty<byte>();
        if (TryHex(item, ref result) || TryDecimal(item, ref result) || TryCharacter(item, ref result) ||
            TryBinary(item, ref result))
        {
            return result;
        }
        
        throw new FormatException("Item in the array is of wrong format");
    }
    
    private static byte[] ValidateConvertStringItem(string item)
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
            case StringBuilder:
                var bytesItem = ValidateConvertStringItem(item.ToString());
                content.Add(new AuxiliaryObjects.Bytes(bytesItem));
                break;
            default:
                throw new FormatException("Item in the array is neither of type Array or string");
        }
    }
    
    private static void ValidateClosingBracket(char openingBracket, char character)
    {
        if (openingBracket == '\0') throw new FormatException("Opening bracket is missing");
        
        var expectedClosingBracket = GetMatchingBracket.GetMatchingClosingBracket(openingBracket);
        if (character != expectedClosingBracket)
        {
            throw new FormatException($"Closing bracket {character} does NOT match opening bracket");
        }
    }

    private (object item, int currentIndex, int openingBracketsNumber) CreateObject(int currentIndex,
        int openingBracketsNumber, char openingBracket)
    {
        object item = new StringBuilder("");

        List<ArrayContentItem> content = new();

        while (currentIndex <= _lastIndex)
        {
            var character = _input[currentIndex];
            
            if (OpeningBrackets.Contains(character))
            {
                openingBracketsNumber += 1;
                (item, currentIndex, openingBracketsNumber) =
                    CreateObject(currentIndex + 1, openingBracketsNumber, character);
            }
            else if (character == ',')
            {
                FinishItem(item, content);
                item = new StringBuilder("");
                currentIndex += 1;
            }
            else if (ClosingBrackets.Contains(character))
            {
                ValidateClosingBracket(openingBracket, character);
                FinishItem(item, content);
                
                return (new AuxiliaryObjects.Array(content), currentIndex + 1, openingBracketsNumber);
            }
            // else if (char.IsWhiteSpace(character))
            // {
            //     currentIndex += 1;
            // }
            else
            {
                ((StringBuilder) item).Append(character);
                currentIndex += 1;
            }
        }

        if (openingBracket == '\0') throw new FormatException("Closing bracket is missing");

        return (item, currentIndex + 1, openingBracketsNumber);
    }

    private void InputIsArray()
    {
        const int firstIndex = 0;
        if (_lastIndex < firstIndex || !OpeningBrackets.Contains(_input[firstIndex]))
        {
            throw new FormatException("Input is not a valid array input");
        }
    }

    private void CheckFromToIfNested(int openingBracketsNumber, IFormat outputFormat)
    {
        if (openingBracketsNumber >= 2 && (InputFormat is not ByteArray || outputFormat is not ByteArray))
        {
            throw new FormatException("Array is nested, however not both formats are of type array");
        }
    }

    private Array ValidateParseInput(IFormat outputFormat)
    {
        _lastIndex = _input.Length - 1;
        
        InputIsArray();
        
        var (result, _, openingBracketsNumber) = CreateObject(0, 0,
            '\0');

        CheckFromToIfNested(openingBracketsNumber, outputFormat);

        return (Array) result;
    }

    public string ConvertTo(string value, IFormat outputFormat)
    {
        _input = value;
        var parsedInput = ValidateParseInput(outputFormat);
        // Output - something with parsedInput  TODO
        return "Hello :)";
    }
}