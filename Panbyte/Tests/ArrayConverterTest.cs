using System.Text;
using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Tests;

[TestClass]
public class ArrayConverterTest
{
    private readonly ArrayConverter _converter = new ArrayConverter(new ByteArray());

    [TestMethod]
    public void AssignmentTestNotNested()
    {
        var oneToFourArrayCurly1 = Encoding.ASCII.GetBytes(@"{0x01, 2, 0b11, '\x04'}");
        var oneToFourArrayCurly2 = Encoding.ASCII.GetBytes(@"{0x01,2,0b11 ,'\x04' }");
        var oneToFourArraySquare = Encoding.ASCII.GetBytes(@"[0x01, 2, 0b11, '\x04']");
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
        Assert.IsTrue(_converter.ConvertTo(oneToFourArraySquare, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(oneToFourArrayHex));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayRegular, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(oneToFourArrayDec));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayCurly1, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(oneToFourArrayChar));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayCurly1, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(oneToFourArrayBin));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayRegular, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(oneToFourArrayHexRegular));
        Assert.IsTrue(_converter.ConvertTo(oneToFourArrayCurly1, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(oneToFourArrayDecimalSquare));
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
        Assert.IsTrue(_converter.ConvertTo(oneToSixArraySquare, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(oneToSixArrayDecCurly));
        Assert.IsTrue(_converter.ConvertTo(oneToSixArrayMixMix, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(oneToSixArrayDecSquare));
        Assert.IsTrue(_converter.ConvertTo(emptyRegular, new ByteArray()).SequenceEqual(emptyCurly));
        Assert.IsTrue(_converter.ConvertTo(emptyMix, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(emptySquare));
    }

    [TestMethod]
    public void EmptyArrays()
    {
        var curly1 = Encoding.ASCII.GetBytes(@"{}");
        var curly2 = Encoding.ASCII.GetBytes(@"{ }");
        var curly3 = Encoding.ASCII.GetBytes("{ \t   \t }");

        var square1 = Encoding.ASCII.GetBytes(@"[]");
        var square2 = Encoding.ASCII.GetBytes(@"[ ]");
        var square3 = Encoding.ASCII.GetBytes("[\t  \t  ]");

        var regular1 = Encoding.ASCII.GetBytes(@"()");
        var regular2 = Encoding.ASCII.GetBytes(@"( )");
        var regular3 = Encoding.ASCII.GetBytes("(  \t  \t )");

        var curlyNested1 = Encoding.ASCII.GetBytes(@"{{}, {{}}}");
        var curlyNested2 = Encoding.ASCII.GetBytes("{{\t \t }, {\t{}}}");
        var curlyNested3 = Encoding.ASCII.GetBytes("{{}, {{}}\t}");

        var squareNested1 = Encoding.ASCII.GetBytes(@"[[], [[]]]");
        var squareNested2 = Encoding.ASCII.GetBytes("[[\t \t ], [\t[]]]");
        var squareNested3 = Encoding.ASCII.GetBytes("[[], [[]]\t]");

        var regularNested1 = Encoding.ASCII.GetBytes(@"((), (()))");
        var regularNested2 = Encoding.ASCII.GetBytes("((\t \t ), (\t()))");
        var regularNested3 = Encoding.ASCII.GetBytes("((), (())\t)");

        var mixNested1 = Encoding.ASCII.GetBytes(@"{({}), {[]}}");
        var mixNested2 = Encoding.ASCII.GetBytes("{({   }\t ), {\t[]}}");
        var mixNested3 = Encoding.ASCII.GetBytes("{({   }), {\t  [\t]} }");

        var mixNestedCurly = Encoding.ASCII.GetBytes(@"{{{}}, {{}}}");
        var mixNestedRegular = Encoding.ASCII.GetBytes(@"((()), (()))");
        var mixNestedSquare = Encoding.ASCII.GetBytes(@"[[[]], [[]]]");

        // The BEGINNING of tests having an input as curly<n>, where n = 1/2/3

        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray()).SequenceEqual(curly1));
        Assert.IsTrue(
            _converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Hex, Brackets.Curly)).SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curly1));

        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(square1));

        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(curly1, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regular1));

        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray()).SequenceEqual(curly1));
        Assert.IsTrue(
            _converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Hex, Brackets.Curly)).SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curly1));

        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(square1));

        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(curly2, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regular1));

        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray()).SequenceEqual(curly1));
        Assert.IsTrue(
            _converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Hex, Brackets.Curly)).SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curly1));

        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(square1));

        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(curly3, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regular1));

        // The END of tests having an input as curly<n>, where n = 1/2/3

        // The BEGINNING of tests having an input as square<n>, where n = 1/2/3

        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray()).SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curly1));

        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(square1));

        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(square1, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regular1));

        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray()).SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curly1));

        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(square1));

        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(square2, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regular1));

        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray()).SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curly1));

        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(square1));

        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(square3, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regular1));

        // The END of tests having an input as square<n>, where n = 1/2/3

        // The BEGINNING of tests having an input as regular<n>, where n = 1/2/3

        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray()).SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curly1));

        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(square1));

        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(regular1, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regular1));

        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray()).SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curly1));

        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(square1));

        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(regular2, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regular1));

        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray()).SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curly1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curly1));

        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(square1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(square1));

        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regular1));
        Assert.IsTrue(_converter.ConvertTo(regular3, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regular1));

        // The END of tests having an input as regular<n>, where n = 1/2/3

        // The BEGINNING of tests having an input as curlyNested<n>, where n = 1/2/3

        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray()).SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curlyNested1));

        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(squareNested1));

        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested1, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regularNested1));

        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray()).SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curlyNested1));

        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(squareNested1));

        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested2, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regularNested1));

        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray()).SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curlyNested1));

        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(squareNested1));

        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(curlyNested3, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regularNested1));

        // The END of tests having an input as curlyNested<n>, where n = 1/2/3

        // The BEGINNING of tests having an input as squareNested<n>, where n = 1/2/3

        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray()).SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curlyNested1));

        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(squareNested1));

        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested1, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regularNested1));

        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray()).SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curlyNested1));

        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(squareNested1));

        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested2, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regularNested1));

        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray()).SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curlyNested1));

        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(squareNested1));

        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(squareNested3, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regularNested1));

        // The END of tests having an input as squareNested<n>, where n = 1/2/3

        // The BEGINNING of tests having an input as regularNested<n>, where n = 1/2/3

        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray()).SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curlyNested1));

        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(squareNested1));

        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested1, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regularNested1));

        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray()).SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curlyNested1));

        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(squareNested1));

        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested2, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regularNested1));

        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray()).SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Decimal, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(curlyNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(curlyNested1));

        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Decimal, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(squareNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(squareNested1));

        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Decimal, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(regularNested1));
        Assert.IsTrue(_converter.ConvertTo(regularNested3, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(regularNested1));

        // The END of tests having an input as regularNested<n>, where n = 1/2/3

        // The BEGINNING of test having an input as mixNested<n>, where n = 1/2/3

        Assert.IsTrue(_converter.ConvertTo(mixNested1, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(mixNestedCurly));
        Assert.IsTrue(_converter.ConvertTo(mixNested1, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(mixNestedCurly));
        Assert.IsTrue(_converter.ConvertTo(mixNested1, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(mixNestedCurly));
        Assert.IsTrue(_converter.ConvertTo(mixNested2, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(mixNestedCurly));
        Assert.IsTrue(_converter.ConvertTo(mixNested2, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(mixNestedCurly));
        Assert.IsTrue(_converter.ConvertTo(mixNested2, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(mixNestedCurly));
        Assert.IsTrue(_converter.ConvertTo(mixNested3, new ByteArray(ArrayFormat.Binary, Brackets.Curly))
            .SequenceEqual(mixNestedCurly));
        Assert.IsTrue(_converter.ConvertTo(mixNested3, new ByteArray(ArrayFormat.Hex, Brackets.Curly))
            .SequenceEqual(mixNestedCurly));
        Assert.IsTrue(_converter.ConvertTo(mixNested3, new ByteArray(ArrayFormat.Char, Brackets.Curly))
            .SequenceEqual(mixNestedCurly));

        Assert.IsTrue(_converter.ConvertTo(mixNested1, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(mixNestedSquare));
        Assert.IsTrue(_converter.ConvertTo(mixNested1, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(mixNestedSquare));
        Assert.IsTrue(_converter.ConvertTo(mixNested1, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(mixNestedSquare));
        Assert.IsTrue(_converter.ConvertTo(mixNested2, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(mixNestedSquare));
        Assert.IsTrue(_converter.ConvertTo(mixNested2, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(mixNestedSquare));
        Assert.IsTrue(_converter.ConvertTo(mixNested2, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(mixNestedSquare));
        Assert.IsTrue(_converter.ConvertTo(mixNested3, new ByteArray(ArrayFormat.Binary, Brackets.Square))
            .SequenceEqual(mixNestedSquare));
        Assert.IsTrue(_converter.ConvertTo(mixNested3, new ByteArray(ArrayFormat.Hex, Brackets.Square))
            .SequenceEqual(mixNestedSquare));
        Assert.IsTrue(_converter.ConvertTo(mixNested3, new ByteArray(ArrayFormat.Char, Brackets.Square))
            .SequenceEqual(mixNestedSquare));

        Assert.IsTrue(_converter.ConvertTo(mixNested1, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(mixNestedRegular));
        Assert.IsTrue(_converter.ConvertTo(mixNested1, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(mixNestedRegular));
        Assert.IsTrue(_converter.ConvertTo(mixNested1, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(mixNestedRegular));
        Assert.IsTrue(_converter.ConvertTo(mixNested2, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(mixNestedRegular));
        Assert.IsTrue(_converter.ConvertTo(mixNested2, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(mixNestedRegular));
        Assert.IsTrue(_converter.ConvertTo(mixNested2, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(mixNestedRegular));
        Assert.IsTrue(_converter.ConvertTo(mixNested3, new ByteArray(ArrayFormat.Binary, Brackets.Regular))
            .SequenceEqual(mixNestedRegular));
        Assert.IsTrue(_converter.ConvertTo(mixNested3, new ByteArray(ArrayFormat.Hex, Brackets.Regular))
            .SequenceEqual(mixNestedRegular));
        Assert.IsTrue(_converter.ConvertTo(mixNested3, new ByteArray(ArrayFormat.Char, Brackets.Regular))
            .SequenceEqual(mixNestedRegular));

        // The END of test having an input as mixNested<n>, where n = 1/2/3
    }

    [TestMethod]
    public void ArraysEdgeCases()
    {
        Assert.ThrowsException<FormatException>(() =>
        {
            _converter.ConvertTo(Encoding.ASCII.GetBytes(@"{()123}"), new ByteArray());
        });
        Assert.ThrowsException<FormatException>(() =>
        {
            _converter.ConvertTo(Encoding.ASCII.GetBytes(@"{123()}"), new ByteArray());
        });
        Assert.ThrowsException<FormatException>(() =>
        {
            _converter.ConvertTo(Encoding.ASCII.GetBytes(@"{(123,)}"), new ByteArray());
        });
        Assert.ThrowsException<FormatException>(() =>
        {
            _converter.ConvertTo(Encoding.ASCII.GetBytes("{123\t,   234 ,\t)}"), new ByteArray());
        });
        
        Assert.ThrowsException<FormatException>(() =>
        {
            _converter.ConvertTo(Encoding.ASCII.GetBytes("{123, 200, 256}"), new ByteArray());
        });
    }
}