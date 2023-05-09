using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;
using System.Text;

namespace Tests;

[TestClass]
public class HexConverterTest
{
    private readonly HexConverter _converter = new(new Hex());

    [TestMethod]
    public void ConvertEmptyHexString()
    {
        
        var emptyBytes = Array.Empty<byte>();
        var emptyCurlyBraces = Encoding.ASCII.GetBytes("{}");
        var emptyRoundBraces = Encoding.ASCII.GetBytes("()");
        var emptySquareBraces = Encoding.ASCII.GetBytes("[]");
        
        Assert.IsTrue( _converter.ConvertTo( emptyBytes, new Bits()).SequenceEqual(emptyBytes));
        Assert.IsTrue(_converter.ConvertTo( emptyBytes, new Bytes()).SequenceEqual(emptyBytes));
        Assert.IsTrue(_converter.ConvertTo( emptyBytes, new Hex()).SequenceEqual(emptyBytes));
        Assert.IsTrue(_converter.ConvertTo( emptyBytes, new Int()).SequenceEqual(emptyBytes));


        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Binary, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Binary, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Binary, Brackets.Square)).SequenceEqual(emptySquareBraces));

        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Hex, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Hex, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Hex, Brackets.Square)).SequenceEqual(emptySquareBraces));
        
        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Square)).SequenceEqual(emptySquareBraces));
        
        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Char, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue( 
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Char, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue(
            _converter.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Char, Brackets.Square)).SequenceEqual(emptySquareBraces));
    }

    [TestMethod]
    public void ConvertShortHexString()
    {
        var testHexString = Encoding.ASCII.GetBytes("74657374");
        var output = Encoding.ASCII.GetBytes("test");
        Assert.IsTrue(_converter.ConvertTo(testHexString, new Bytes()).SequenceEqual(output));
    }

    [TestMethod]
    public void ConvertShortHexStringWithSpaces()
    {
        var testHexString = Encoding.ASCII.GetBytes("74 65 \n    73\r74");
        var output = Encoding.ASCII.GetBytes("test");
        Assert.IsTrue(_converter.ConvertTo(testHexString, new Bytes()).SequenceEqual(output));
    }
}