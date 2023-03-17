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
    }
}