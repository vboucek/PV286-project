using System.Text;
using System.Text.RegularExpressions;
using Panbyte.Converters.AuxiliaryObjects;
using Panbyte.Formats;
using Panbyte.Utils;
using Array = Panbyte.Converters.AuxiliaryObjects.Array;

namespace Panbyte.Converters;

/// <summary>
/// Converter for converting from byte array format.
/// </summary>
public class ArrayConverter : ByteSequenceConverterBase, IConverter
{
    public ArrayConverter(ByteArray inputFormat)
    {
        InputFormat = inputFormat;
    }

    public Format InputFormat { get; }

    private byte[]? Input { get; set; }
    private int LastIndex { get; set; }

    private static readonly HashSet<byte> OpeningBrackets =
        new() { Convert.ToByte('{'), Convert.ToByte('['), Convert.ToByte('(') };

    private static readonly HashSet<byte> ClosingBrackets =
        new() { Convert.ToByte('}'), Convert.ToByte(']'), Convert.ToByte(')') };

    /// <summary>
    /// Try, and if successful, convert Byte array item to hex format
    /// </summary>
    /// <param name="item">item that is checked and converted</param>
    /// <param name="result">output of conversion</param>
    /// <returns>true if the conversion was successful</returns>
    private static bool TryHex(string item, ref byte result)
    {
        var rgxHex = new Regex(@"^0x[\da-f][\da-f]$");
        if (!rgxHex.IsMatch(item)) return false;
        var resultArrayBytes = Convert.FromHexString(item.Substring(2, 2));
        result = resultArrayBytes[0]; // item will never be more than 1 byte
        return true;
    }

    /// <summary>
    /// Try, and if successful, convert Byte array item to decimal format
    /// </summary>
    /// <param name="item">item that is checked and converted</param>
    /// <param name="result">output of conversion</param>
    /// <returns>true if the conversion was successful</returns>
    /// <exception cref="FormatException">when decimal number has more than one byte</exception>
    private static bool TryDecimal(string item, ref byte result)
    {
        if (!Decimal.TryParse(item, out var resDecimal) || resDecimal < 0) return false;
        if (resDecimal >= 256)
            throw new FormatException($"Invalid array format. Item {resDecimal} is more than 1 byte");
        result = Decimal.ToByte(resDecimal);
        return true;
    }

    /// <summary>
    /// Try, and if successful, convert Byte array item to character format
    /// </summary>
    /// <param name="item">item that is checked and converted</param>
    /// <param name="result">output of conversion</param>
    /// <returns>true if the conversion was successful</returns>
    /// <exception cref="FormatException">when character has more than one byte</exception>
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
            var ord = (int)item[1];
            if (ord is < 0 or > 255)
                throw new FormatException($"Invalid array format. Item {item[1]} is more than 1 byte");
            result = Convert.ToByte(item[1]);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Try, and if successful, convert Byte array item to binary format
    /// </summary>
    /// <param name="item">item that is checked and converted</param>
    /// <param name="result">output of conversion</param>
    /// <returns>true if the conversion was successful</returns>
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

    /// <summary>
    /// Try, and if successful, convert Byte array item to one of allowed formats
    /// </summary>
    /// <param name="item">item that is checked and converted</param>
    /// <returns>converted bytes from the input</returns>
    /// <exception cref="FormatException">when item bytes do not correspond to neither of allowed formats</exception>
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

    /// <summary>
    /// Validate if bytes of Byte array item form an allowed format and convert it if valid
    /// </summary>
    /// <param name="item">item that is checked and converted</param>
    /// <returns>converted bytes from the input</returns>
    private static byte ValidateConvertStringItem(string item)
    {
        var stripedItemString = item.Trim();
        return MatchConvertToFormat(stripedItemString);
    }

    /// <summary>
    /// Validate if the closing bracket match the opening one
    /// </summary>
    /// <param name="openingBracket">byte corresponding to the opening bracket</param>
    /// <param name="charByte">byte corresponding to the closing bracket</param>
    /// <exception cref="FormatException">when the opening bracket is missing or when the opening and the closing brackets do not match</exception>
    private static void ValidateClosingBracket(byte openingBracket, byte charByte)
    {
        if (openingBracket == Convert.ToByte('\0')) throw new FormatException("Opening bracket is missing");

        var expectedClosingBracket = GetMatchingBracket.GetMatchingClosingBracket(openingBracket);
        if (charByte != expectedClosingBracket)
        {
            throw new FormatException($"Closing bracket {charByte} does NOT match opening bracket {openingBracket}");
        }
    }

    /// <summary>
    /// Validate and converts list of bytes to .Content attribute in new instance of class AuxiliaryObjects.Byte
    /// </summary>
    /// <param name="currentByte">list of bytes that will be validated, converted and used in new instance of class AuxiliaryObjects.Byte</param>
    /// <returns>instance of class AuxiliaryObjects.Byte</returns>
    /// <exception cref="NullReferenceException">when input is null</exception>
    private AuxiliaryObjects.Byte FinishByte(List<byte> currentByte)
    {
        var stringItem = Encoding.UTF8.GetString(currentByte.ToArray());
        if (stringItem is null) throw new NullReferenceException("Item is null and should not be");
        var bytesItem = ValidateConvertStringItem(stringItem);
        return new AuxiliaryObjects.Byte(bytesItem);
    }

    /// <summary>
    /// Jumps over all whitespace bytes till the first occurrence of not whitespace byte
    /// </summary>
    /// <param name="currentIndex">index of the byte from which "jumping" starts</param>
    /// <returns>index of the first following not whitespace byte</returns>
    private int SkipWhiteSpace(int currentIndex)
    {
        while (currentIndex <= LastIndex)
        {
            if (char.IsWhiteSpace(Convert.ToChar(Input![currentIndex])))
            {
                currentIndex++;
            }
            else
            {
                break;
            }
        }

        return currentIndex;
    }

    /// <summary>
    /// Parse input into an instance of class AuxiliaryObjects.Array
    /// </summary>
    /// <param name="currentIndex">index of byte, in the byte array input, that will be processed</param>
    /// <param name="openingBracketsNumber">number of opening brackets, in the byte array input, processed so far</param>
    /// <param name="openingBracket">byte corresponding to the opening bracket</param>
    /// <param name="fstItmInArray">bool indicating whether the byte is the first item in the array</param>
    /// <param name="recCallNumber">number of recursive calls made so far</param>
    /// <returns>instance of class AuxiliaryObjects.Array corresponding to the Byte array input</returns>
    /// <exception cref="StackOverflowException">when input array is too nested</exception>
    /// <exception cref="FormatException">when the input array format is invalid</exception>
    private (Array array, int currentIndex, int openingBracketsNumber) CreateArray(int currentIndex,
        int openingBracketsNumber, byte openingBracket, bool fstItmInArray, int recCallNumber)
    {
        var finishedItem = false;
        List<ArrayContentItem> content = new();

        if (recCallNumber >= 1000) throw new StackOverflowException("Too nested array");

        List<byte>? currentByte = null;

        currentIndex = SkipWhiteSpace(currentIndex);

        while (currentIndex <= LastIndex)
        {
            var charByte = Input![currentIndex];

            if (recCallNumber == 0 && !OpeningBrackets.Contains(charByte))
            {
                throw new FormatException("Invalid start of the array.");
            }

            if (OpeningBrackets.Contains(charByte))
            {
                if (currentByte is not null)
                    throw new FormatException("Invalid array format.");

                if (finishedItem)
                    throw new FormatException("Invalid array format. One of the opening brackets is in the wrong spot");

                openingBracketsNumber++;
                (var newArray, currentIndex, openingBracketsNumber) =
                    CreateArray(currentIndex + 1, openingBracketsNumber, charByte, true, recCallNumber + 1);

                if (recCallNumber == 0)
                {
                    currentIndex = SkipWhiteSpace(currentIndex);

                    if (currentIndex != Input.Length)
                    {
                        throw new FormatException("Invalid array format.");
                    }

                    return (newArray, currentIndex + 1, openingBracketsNumber);
                }

                content.Add(newArray);

                finishedItem = true;
            }
            else if (charByte == Convert.ToByte(','))
            {
                if (currentByte is not null && currentByte.Count == 0)
                    throw new FormatException("Invalid array format. Array item can not be empty");

                if (currentByte is null && !finishedItem)
                {
                    throw new FormatException("Array item cannot be empty.");
                }

                if (!finishedItem)
                {
                    content.Add(FinishByte(currentByte!));
                }

                currentByte = null;
                currentIndex++;
                finishedItem = false;
                fstItmInArray = false;

                currentIndex = SkipWhiteSpace(currentIndex);
            }
            else if (ClosingBrackets.Contains(charByte))
            {
                if (!finishedItem && currentByte is null && !fstItmInArray)
                    throw new FormatException("Invalid array format. Array item can not be empty");

                if (!finishedItem && currentByte is not null)
                {
                    content.Add(FinishByte(currentByte));
                }

                ValidateClosingBracket(openingBracket, charByte);

                return (new Array(content), currentIndex + 1, openingBracketsNumber);
            }
            else if (char.IsWhiteSpace(Convert.ToChar(Input![currentIndex])))
            {
                if (currentByte is not null)
                {
                    content.Add(FinishByte(currentByte));
                }

                finishedItem = true;
                currentIndex++;
            }
            else
            {
                if (finishedItem)
                {
                    throw new FormatException("Invalid array format.");
                }

                if (currentByte is null)
                {
                    currentByte = new List<byte>();
                    finishedItem = false;
                }

                currentByte.Add(charByte);
                currentIndex++;
            }
        }

        if (openingBracket != Convert.ToByte('\0'))
            throw new FormatException($"Closing bracket to opening bracket {openingBracket} is missing");

        return (new Array(content), currentIndex + 1, openingBracketsNumber);
    }

    /// <summary>
    /// Check if from and to are set to Byte array in case that input is nested
    /// </summary>
    /// <param name="openingBracketsNumber">number of opening brackets in the input</param>
    /// <param name="outputFormat">the output format</param>
    /// <exception cref="FormatException">when an input is nested and not both formats (input and output) are Byte array</exception>
    private void CheckFromToIfNested(int openingBracketsNumber, Format outputFormat)
    {
        if (openingBracketsNumber >= 2 && (InputFormat is not ByteArray || outputFormat is not ByteArray))
        {
            throw new FormatException("Array is nested, however not both formats are of type array");
        }
    }

    /// <summary>
    /// Validate input and parse it if valid
    /// </summary>
    /// <param name="outputFormat">the output format</param>
    /// <returns>parsed input</returns>
    /// <exception cref="FormatException">when an input is not Byte array format</exception>
    private Array ValidateParseInput(Format outputFormat)
    {
        LastIndex = Input!.Length - 1;

        if (Input.Length == 0)
        {
            throw new FormatException("Input is not an array");
        }

        var (result, _, openingBracketsNumber) = CreateArray(0, 0,
            Convert.ToByte('\0'), false, 0);

        CheckFromToIfNested(openingBracketsNumber, outputFormat);

        return result;
    }

    /// <summary>
    /// Nice print when output format is not Byte array
    /// </summary>
    /// <param name="input">an instance of class AuxiliaryObjects.Array</param>
    /// <param name="outputFormat">the output format</param>
    /// <returns>bytes representing nice print</returns>
    private byte[] OutputNotByteArray(Array input, Format outputFormat)
    {
        var items = new List<byte>();

        foreach (var arrayContItm in input.Content)
        {
            var byteItem = (AuxiliaryObjects.Byte)arrayContItm;
            items.Add(byteItem.Content);
        }

        return BaseConvertTo(items.ToArray(), outputFormat);
    }

    /// <summary>
    /// Converts an array of bytes
    /// </summary>
    /// <param name="value">an array of bytes</param>
    /// <param name="outputFormat">the output format</param>
    /// <returns>bytes of converted output</returns>
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