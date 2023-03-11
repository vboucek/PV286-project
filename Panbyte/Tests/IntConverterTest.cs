using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

[TestClass]
public class IntConverterTest
{
    private readonly IntConverter _converter_big = new IntConverter(new Int());
    private readonly IntConverter _converter_little = new IntConverter(new Int(Endianness.LittleEndian));

    [TestMethod]
    public void ConvertFromBigEndian()
    {
        var testIntString = "1234567890";
        Assert.AreEqual("499602d2", _converter_big.ConvertTo(testIntString, new Hex()));
    }

    [TestMethod]
    public void ConvertFromLittleEndian()
    {
        var testIntString = "1234567890";
        Assert.AreEqual("d2029649", _converter_little.ConvertTo(testIntString, new Hex()));
    }

}