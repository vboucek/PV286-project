using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

[TestClass]
public class BitsConverterTest
{
    /*private readonly BitsConverter _converterLeft = new(new Bits());
    private readonly BitsConverter _converterRight = new(new Bits(BitPadding.Right));

    [TestMethod]
    public void ConvertEmptyBits()
    {
        var emptyBits = "";
        Assert.AreEqual("", _converterLeft.ConvertTo(emptyBits, new Bits()));
        Assert.AreEqual("", _converterLeft.ConvertTo(emptyBits, new Bytes()));
        Assert.AreEqual("", _converterLeft.ConvertTo(emptyBits, new Hex()));
        Assert.AreEqual("", _converterLeft.ConvertTo(emptyBits, new Int()));
        
        
        Assert.AreEqual("{}", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Binary, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Binary, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Binary, Brackets.Square)));

        Assert.AreEqual("{}", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Hex, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Hex, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Hex, Brackets.Square)));
        
        Assert.AreEqual("{}", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Decimal, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Decimal, Brackets.Square)));
        
        Assert.AreEqual("{}", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Char, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Char, Brackets.Regular)));
        Assert.AreEqual("[]",
            _converterLeft.ConvertTo(emptyBits, new ByteArray(ArrayFormat.Char, Brackets.Square)));
    }

    
    [TestMethod]
    public void ConvertBitsWithSpaces()
    {
        var testHexString = "100 1111 0100 1011";
        Assert.AreEqual("OK", _converterLeft.ConvertTo(testHexString, new Bytes()));
    }

    [TestMethod]
    public void ConvertBits()
    {
        var testHexString = "100111101001011";
        Assert.AreEqual("OK", _converterLeft.ConvertTo(testHexString, new Bytes()));
        Assert.AreEqual("9e96", _converterRight.ConvertTo(testHexString, new Hex()));
    } */
}