namespace Tests;

[TestClass]
public class DelimiterTest : RunPanbyteTest
{
    [TestMethod]
    public void EdgeCaseDelimiter()
    {
        RunPanbyteWithConsoleInput("-f bytes -t bytes -d |", "test|test|test", "test|test|test");
        RunPanbyteWithConsoleInput(@"-f bytes -t bytes -d """, "abc", "abc");

        // Bytes -  Don't take delimiter into account, should whole input
        RunPanbyteWithConsoleInput("-f bytes -t bytes", "abc", "abc");

        // Hex - Use newline as a delimiter
        RunPanbyteWithConsoleInput("-f hex -t bytes",
            $"74657374{Environment.NewLine}74657374{Environment.NewLine}74657374",
            $"test{Environment.NewLine}test{Environment.NewLine}test");
    }

    /*
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
    }*/
}