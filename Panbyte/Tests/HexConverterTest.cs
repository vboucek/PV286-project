using Panbyte.Converters;
using Panbyte.Formats;

namespace Tests;

[TestClass]
public class HexConverterTest
{
    private readonly HexConverter _converter = new(new Hex());

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