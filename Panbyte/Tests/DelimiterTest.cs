namespace Tests;

[TestClass]
public class DelimiterTest : RunPanbyteTest
{
    [TestMethod]
    public void EdgeCaseDelimiter()
    {
        var args = new[] { "-f", "bytes", "-t", "bytes", "-d", "|" };
        var testInput = "test|test|test";
        var expectedOutput = $"test|test|test{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));

        args = new[] { "-f", "bytes", "-t", "bytes", "-d", "" };
        testInput = "abc";
        expectedOutput = $"abc{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));

        // Bytes -  Don't take delimiter into account, should whole input
        args = new[] { "-f", "bytes", "-t", "bytes" };
        testInput = "abc";
        expectedOutput = $"abc{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));

        // Hex - Use newline as a delimiter
        args = new[] { "-f", "hex", "-t", "bytes" };
        testInput = $"74657374{Environment.NewLine}74657374{Environment.NewLine}74657374";
        expectedOutput = $"test{Environment.NewLine}test{Environment.NewLine}test{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));
    }

    [TestMethod]
    public void MultiCharDelimiter()
    {
        var args = new[] { "-f", "bytes", "-t", "hex", "-d", "aaa" };
        var testInput = "testaaatest";
        var expectedOutput = $"74657374aaa74657374{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));

        args = new[] { "-f", "bytes", "-t", "hex", "-d", "aaa" };
        testInput = "aaa";
        expectedOutput = $"aaa{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));

        args = new[] { "-f", "bytes", "-t", "hex", "-d", "ttt" };
        testInput = "testtttesttttesttttes";
        expectedOutput = $"746573ttt746573ttt746573ttt746573{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));
    }

    [TestMethod]
    public void ValidMultiCharDelimiter()
    {
        var args = new[] { "-f", "bytes", "-t", "bytes", "-d", "delimiter" };
        var testInput = "delimiterdelimiterdelimiter";
        var expectedOutput = $"delimiterdelimiterdelimiter{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));

        args = new[] { "-f", "bytes", "-t", "hex", "-d", "aaa" };
        testInput = "testaaatest";
        expectedOutput = $"74657374aaa74657374{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));

        args = new[] { "-f", "bytes", "-t", "hex", "-d", "aaa" };
        testInput = "aaa";
        expectedOutput = $"aaa{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));

        args = new[] { "-f", "bytes", "-t", "hex", "-d", "ttt" };
        testInput = "testtttesttttesttttes";
        expectedOutput = $"746573ttt746573ttt746573ttt746573{Environment.NewLine}";

        Assert.AreEqual(expectedOutput, RunPanbyteWithConsoleInput(testInput, args));
    }
}