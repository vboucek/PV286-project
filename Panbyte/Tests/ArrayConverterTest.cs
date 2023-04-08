using System.Text;
using System.Linq;
using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

[TestClass]
public class ArrayConverterTest
{
    private readonly ArrayConverter _converter = new ArrayConverter();

    [TestMethod]
    public void AssignmentTestNotNested()
    {
        var oneToFourArrayCurly1 = Encoding.ASCII.GetBytes(@"{0x01, 2, 0b11, '\x04'}");
        var oneToFourArrayCurly2 = Encoding.ASCII.GetBytes(@"{0x01,2,0b11 ,'\x04' }");
        var oneToFourArraySquare= Encoding.ASCII.GetBytes(@"[0x01, 2, 0b11, '\x04']");
        var oneToFourArrayRegular = Encoding.ASCII.GetBytes(@"(0x01, 2, 0b11, '\x04')");
        
        var oneToFourHex = Encoding.ASCII.GetBytes(@"01020304");
        var oneToFourArrayHex = Encoding.ASCII.GetBytes(@"{0x1, 0x2, 0x3, 0x4}");
        var oneToFourArrayDec = Encoding.ASCII.GetBytes(@"{1, 2, 3, 4}");
        var oneToFourArrayChar = Encoding.ASCII.GetBytes(@"{'\x01', '\x02', '\x03', '\x04'}");
        var oneToFourArrayBin = Encoding.ASCII.GetBytes(@"{0b1, 0b10, 0b11, 0b100}");
        var oneToFourArrayHexRegular = Encoding.ASCII.GetBytes(@"(0x1, 0x2, 0x3, 0x4)");
        var oneToFourArrayDecimalSquare = Encoding.ASCII.GetBytes(@"[1, 2, 3, 4]");

        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayCurly1, new Hex()).SequenceEqual(oneToFourHex));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayCurly2, new ByteArray()).SequenceEqual(oneToFourArrayHex));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArraySquare, new ByteArray(ArrayFormat.Hex, Brackets.Curly)).SequenceEqual(oneToFourArrayHex));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayRegular, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)).SequenceEqual(oneToFourArrayDec));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayCurly1, new ByteArray(ArrayFormat.Char, Brackets.Curly)).SequenceEqual(oneToFourArrayChar));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayCurly1, new ByteArray(ArrayFormat.Binary, Brackets.Curly)).SequenceEqual(oneToFourArrayBin));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayRegular, new ByteArray(ArrayFormat.Hex, Brackets.Regular)).SequenceEqual(oneToFourArrayHexRegular));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayCurly1, new ByteArray(ArrayFormat.Decimal, Brackets.Square)).SequenceEqual(oneToFourArrayDecimalSquare));
    }
    
    [TestMethod]
    public void AssignmentTestNested()
    {
        var oneToSixArraySquare = Encoding.ASCII.GetBytes(@"[[1, 2], [3, 4], [5, 6]]");
        var oneToSixArrayMixMix = Encoding.ASCII.GetBytes(@"{{0x01, (2), [3, 0b100, 0x05], '\x06'}}");
        var emptyRegular = Encoding.ASCII.GetBytes(@"()");
        var emptyMix = Encoding.ASCII.GetBytes(@"([],{})");
        
        var oneToSixArrayHexCurly = Encoding.ASCII.GetBytes(@"{{0x1, 0x2}, {0x3, 0x4}, {0x5, 0x6}}");
        var oneToSixArrayDecCurly = Encoding.ASCII.GetBytes(@"{{1, 2}, {3, 4}, {5, 6}}");
        var oneToSixArrayDecSquare = Encoding.ASCII.GetBytes(@"[[1, [2], [3, 4, 5], 6]]");
        var emptyCurly = Encoding.ASCII.GetBytes(@"{}");
        var emptySquare = Encoding.ASCII.GetBytes(@"[[], []]");

        Assert.IsTrue(_converter.ConvertTo(oneToSixArraySquare, new ByteArray()).SequenceEqual(oneToSixArrayHexCurly));
        Assert.IsTrue(_converter.ConvertTo(oneToSixArraySquare, new ByteArray(ArrayFormat.Decimal, Brackets.Curly)).SequenceEqual(oneToSixArrayDecCurly));
        Assert.IsTrue(_converter.ConvertTo(oneToSixArrayMixMix, new ByteArray(ArrayFormat.Decimal, Brackets.Square)).SequenceEqual(oneToSixArrayDecSquare));
        Assert.IsTrue(_converter.ConvertTo(emptyRegular, new ByteArray()).SequenceEqual(emptyCurly));
        Assert.IsTrue(_converter.ConvertTo(emptyMix, new ByteArray()).SequenceEqual(emptySquare));
    }
} 
