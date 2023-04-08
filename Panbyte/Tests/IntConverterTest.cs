using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;
using System.Text;
using System.Linq;

namespace Tests;

[TestClass]
public class IntConverterTest
{
    private readonly IntConverter _converterBig = new(new Int());
    private readonly IntConverter _converterLittle = new(new Int(Endianness.LittleEndian));

    [TestMethod]
    public void ConvertEmptyHexString()
    {
        var emptyBytes = Array.Empty<byte>();
        var emptyCurlyBraces = Encoding.ASCII.GetBytes("{}");
        var emptyRoundBraces = Encoding.ASCII.GetBytes("()");
        var emptySquareBraces = Encoding.ASCII.GetBytes("[]");
        
        
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Binary, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Binary, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Binary, Brackets.Square)).SequenceEqual(emptySquareBraces));

        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Hex, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Hex, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Hex, Brackets.Square)).SequenceEqual(emptySquareBraces));
        
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Decimal, Brackets.Square)).SequenceEqual(emptySquareBraces));
        
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Char, Brackets.Curly)).SequenceEqual(emptyCurlyBraces));
        Assert.IsTrue( 
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Char, Brackets.Regular)).SequenceEqual(emptyRoundBraces));
        Assert.IsTrue(
            _converterBig.ConvertTo( emptyBytes, new ByteArray(ArrayFormat.Char, Brackets.Square)).SequenceEqual(emptySquareBraces));
    }
    
    [TestMethod]
    public void ConvertFromBigEndian()
    {
        var testIntString = Encoding.ASCII.GetBytes("1234567890");
        var output = Encoding.ASCII.GetBytes("499602d2");
        Assert.IsTrue(_converterBig.ConvertTo(testIntString, new Hex()).SequenceEqual(output));
    }

    [TestMethod]
    public void ConvertFromLittleEndian()
    {
        var testIntString = Encoding.ASCII.GetBytes("1234567890");
        var output = Encoding.ASCII.GetBytes("d2029649");
        Assert.IsTrue(_converterLittle.ConvertTo(testIntString, new Hex()).SequenceEqual(output));
    }

}