using System.Text;
using System.Linq;
using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

[TestClass]
public class BitsConverterTest
{
    private readonly BitsConverter _converterLeft = new(new Bits());
    private readonly BitsConverter _converterRight = new(new Bits(BitPadding.Right));

    [TestMethod]
    public void ConvertEmptyBits()
    {
        var emptyBytes = Array.Empty<byte>();
        var emptyCurlyBraces = Encoding.ASCII.GetBytes("{}");
        var emptyRoundBraces = Encoding.ASCII.GetBytes("()");
        var emptySquareBraces = Encoding.ASCII.GetBytes("[]");
        
        
        
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new Bits()).SequenceEqual(emptyBytes));
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new Bits()).SequenceEqual(emptyBytes));;
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new Bits()).SequenceEqual(emptyBytes));;
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new Bits()).SequenceEqual(emptyBytes));;
        
        
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Binary, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Binary, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Binary, Brackets.Square)).SequenceEqual(emptySquareBraces));

        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Hex, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Hex, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Hex, Brackets.Square)).SequenceEqual(emptySquareBraces));
        
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Square)).SequenceEqual(emptySquareBraces));
        
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Char, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Char, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue(_converterLeft.ConvertTo(emptyBytes, new ByteArray(ArrayFormat.Char, Brackets.Square)).SequenceEqual(emptySquareBraces));
    }

    
    [TestMethod]
    public void ConvertBitsWithSpaces()
    {
        var testBitString = Encoding.ASCII.GetBytes("100 1111 0100 1011");
        var output = Encoding.ASCII.GetBytes("OK");
        Assert.IsTrue(_converterLeft.ConvertTo(testBitString, new Bytes()).SequenceEqual(output));
    }

    [TestMethod]
    public void ConvertBits()
    {
        var testBitString = Encoding.ASCII.GetBytes("100111101001011");
        var outputText = Encoding.ASCII.GetBytes("OK");
        var outputHexString = Encoding.ASCII.GetBytes("9e96");
        
        
        Assert.IsTrue(_converterLeft.ConvertTo(testBitString, new Bytes()).SequenceEqual(outputText));
        Assert.IsTrue(_converterRight.ConvertTo(testBitString, new Hex()).SequenceEqual(outputHexString));
    }
} 