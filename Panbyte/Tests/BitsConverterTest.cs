using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

public class BitsConverterTest
{
    private readonly BitsConverter _converter_left = new BitsConverter(new Bits());
    private readonly BitsConverter _converter_right = new BitsConverter(new Bits(BitPadding.Right));

    [TestMethod]
    public void ConvertBitsWithSpaces()
    {
        var testHexString = "100 1111 0100 1011";
        Assert.AreEqual("OK", _converter_left.ConvertTo(testHexString, new Bytes()));
    }

    [TestMethod]
    public void ConvertBits()
    {
        var testHexString = "100111101001011";
        Assert.AreEqual("OK", _converter_left.ConvertTo(testHexString, new Bytes()));
        Assert.AreEqual("9e96", _converter_right.ConvertTo(testHexString, new Hex()));
    }
}