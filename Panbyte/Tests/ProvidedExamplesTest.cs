namespace Tests;

[TestClass]
public class ProvidedExamplesTest : RunPanbyteTest
{
    [TestMethod]
    public void BytesTest()
    {
        var args = new[] { "-f", "bytes", "-t", "bytes" };
        var testInput = "test";
        var expectedOutput = $"test{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
    }

    [TestMethod]
    public void HexTest()
    {
        var args = new[] { "-f", "hex", "-t", "bytes" };
        var testInput = "74657374";
        var expectedOutput = $"test{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "bytes", "-t", "hex" };
        testInput = "test";
        expectedOutput = $"74657374{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "hex", "-t", "bytes" };
        testInput = "74 65 73 74";
        expectedOutput = $"test{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
    }

    [TestMethod]
    public void IntTest()
    {
        var args = new[] { "-f", "int", "-t", "hex" };
        var testInput = "1234567890";
        var expectedOutput = $"499602d2{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "int", "--from-options=big", "-t", "hex" };
        testInput = "1234567890";
        expectedOutput = $"499602d2{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "int", "--from-options=little", "-t", "hex" };
        testInput = "1234567890";
        expectedOutput = $"d2029649{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "hex", "-t", "int" };
        testInput = "499602d2";
        expectedOutput = $"1234567890{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "hex", "-t", "int", "--to-options=big" };
        testInput = "499602d2";
        expectedOutput = $"1234567890{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "hex", "-t", "int", "--to-options=little" };
        testInput = "d2029649";
        expectedOutput = $"1234567890{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
    }

    [TestMethod]
    public void BitsTest()
    {
        var args = new[] { "-f", "bits", "-t", "bytes" };
        var testInput = "100 1111 0100 1011";
        var expectedOutput = $"OK{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "bits", "--from-options=left", "-t", "bytes" };
        testInput = "100111101001011";
        expectedOutput = $"OK{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "bits", "--from-options=right", "-t", "hex" };
        testInput = "100111101001011";
        expectedOutput = $"9e96{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "bytes", "-t", "bits" };
        testInput = "OK";
        expectedOutput = $"0100111101001011{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
    }

    // to be implemented
    /*
    [TestMethod]
    public void ByteArrayTest()
    {
        var args = new[] { "-f", "hex", "-t", "array" };
        var testInput = "01020304";
        var expectedOutput = "{0x1, 0x2, 0x3, 0x4}" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "array", "-t", "hex" };
        testInput = @"{0x01, 2, 0b11, '\x04'}";
        expectedOutput = $"01020304{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "array", "-t", "array" };
        testInput = @"{0x01,2,0b11 ,'\x04' }";
        expectedOutput = "{0x1, 0x2, 0x3, 0x4}" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "array", "-t", "array", "--to-options=0x" };
        testInput = @"[0x01, 2, 0b11, '\x04']";
        expectedOutput = "{0x1, 0x2, 0x3, 0x4}" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "array", "-t", "array", "--to-options=0" };
        testInput = @"(0x01, 2, 0b11, '\x04')";
        expectedOutput = "{1, 2, 3, 4}" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
        
        args = new[] { "-f", "array", "-t", "array", "--to-options=a" };
        testInput = @"{0x01, 2, 0b11, '\x04'}";
        expectedOutput = @"{'\x01', '\x02', '\x03', '\x04'}" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
                
        args = new[] { "-f", "array", "-t", "array", "--to-options=0b" };
        testInput = @"[0x01, 2, 0b11, '\x04']";
        expectedOutput = @"{0b1, 0b10, 0b11, 0b100}" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
        
        args = new[] { "-f", "array", "-t", "array", "--to-options=(" };
        testInput = @"(0x01, 2, 0b11, '\x04')";
        expectedOutput = "(0x1, 0x2, 0x3, 0x4)" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
        
        args = new[] { "-f", "array", "-t", "array", "--to-options=0", "--to-options=[" };
        testInput = @"{0x01, 2, 0b11, '\x04'}";
        expectedOutput = "[1, 2, 3, 4]" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));

        args = new[] { "-f", "array", "-t", "array" };
        testInput = "[[1, 2], [3, 4], [5, 6]]";
        expectedOutput = "{{0x1, 0x2}, {0x3, 0x4}, {0x5, 0x6}}" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
        
        args = new[] { "-f", "array", "-t", "array", "--to-options={", "--to-options=0" };
        testInput = "[[1, 2], [3, 4], [5, 6]]";
        expectedOutput = "{{1, 2}, {3, 4}, {5, 6}}" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
        
        args = new[] { "-f", "array", "-t", "array", "--to-options=0", "--to-options=[" };
        testInput = @"{{0x01, (2), [3, 0b100, 0x05], '\x06'}}";
        expectedOutput = "[[1, [2], [3, 4, 5], 6]]" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
        
        args = new[] { "-f", "array", "-t", "array" };
        testInput = "()";
        expectedOutput = "{}" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
        
        args = new[] { "-f", "array", "-t", "array", "--to-options=[" };
        testInput = "([],{})";
        expectedOutput = "[[], []]" + Environment.NewLine;

        Assert.AreEqual(expectedOutput, RunPanbyte(testInput, args));
    } */
}