using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

[TestClass]
public class IntConverterTest
{
    /*private readonly IntConverter _converterBig = new(new Int());
    private readonly IntConverter _converterLittle = new(new Int(Endianness.LittleEndian));

    [TestMethod]
    public void ConvertEmptyHexString()
    {
        var emptyHexString = "";
        Assert.AreEqual("", _converterBig.ConvertTo( emptyHexString, new Bits()));
        Assert.AreEqual("", _converterBig.ConvertTo( emptyHexString, new Bytes()));
        Assert.AreEqual("", _converterBig.ConvertTo( emptyHexString, new Hex()));
        Assert.AreEqual("", _converterBig.ConvertTo( emptyHexString, new Int()));
        
        
        Assert.AreEqual("{}", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Binary, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Binary, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Binary, Brackets.Square)));

        Assert.AreEqual("{}", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Hex, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Hex, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Hex, Brackets.Square)));
        
        Assert.AreEqual("{}", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Decimal, Brackets.Regular)));
        Assert.AreEqual("[]", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Decimal, Brackets.Square)));
        
        Assert.AreEqual("{}", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Char, Brackets.Curly)));
        Assert.AreEqual("()", 
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Char, Brackets.Regular)));
        Assert.AreEqual("[]",
            _converterBig.ConvertTo( emptyHexString, new ByteArray(ArrayFormat.Char, Brackets.Square)));
    }
    
    [TestMethod]
    public void ConvertFromBigEndian()
    {
        var testIntString = "1234567890";
        Assert.AreEqual("499602d2", _converterBig.ConvertTo(testIntString, new Hex()));
    }

    [TestMethod]
    public void ConvertFromLittleEndian()
    {
        var testIntString = "1234567890";
        Assert.AreEqual("d2029649", _converterLittle.ConvertTo(testIntString, new Hex()));
    }
*/
}