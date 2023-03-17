using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

[TestClass]
public class IntConverterTest
{
    private readonly IntConverter _converterBig = new(new Int());
    private readonly IntConverter _converterLittle = new(new Int(Endianness.LittleEndian));

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

}