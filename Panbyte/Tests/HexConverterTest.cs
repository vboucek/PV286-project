using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

[TestClass]
public class HexConverterTest
{
    private readonly HexConverter _converter = new(new Hex());

    [TestMethod]
    public void ConvertEmptyHexString()
    {
        
        var emptyHexString = "";
        Assert.AreEqual("", _converter.ConvertTo( emptyHexString, new Bits()));
        Assert.AreEqual("", _converter.ConvertTo( emptyHexString, new Bytes()));
        Assert.AreEqual("", _converter.ConvertTo( emptyHexString, new Hex()));
        Assert.AreEqual("", _converter.ConvertTo( emptyHexString, new Int()));
        
        
        Assert.AreEqual("{}", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Binary, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Binary, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Binary, Brackets.Square)));

        Assert.AreEqual("{}", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Hex, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Hex, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Hex, Brackets.Square)));
        
        Assert.AreEqual("{}", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Decimal, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Decimal, Brackets.Square)));
        
        Assert.AreEqual("{}", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Char, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Char, Brackets.Regular)));
        Assert.AreEqual("[]",
            _converter.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Char, Brackets.Square)));
    }

    [TestMethod]
    public void ConvertShortHexString()
    {
        var testHexString = "74657374";
        Assert.AreEqual("test", _converter.ConvertTo(testHexString, new Bytes()));
    }

    [TestMethod]
    public void ConvertShortHexStringWithSpaces()
    {
        var testHexString = "74 65 \n    73\r74";
        Assert.AreEqual("test", _converter.ConvertTo(testHexString, new Bytes()));
    }
}