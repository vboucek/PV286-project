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

    /*[TestMethod]
    public void MultiCharDelimiter()
    {
        RunPanbyteWithConsoleInput("-f bytes -t hex -d aaa", "testaaatest", "74657374aaa74657374");
        RunPanbyteWithConsoleInput("-f bytes -t hex -d aaa", "aaa", "aaa");
        RunPanbyteWithConsoleInput("-f bytes -t hex -d ttt", "testtttesttttesttttes",
            "746573ttt746573ttt746573ttt746573");
    }

    [TestMethod]
    public void ValidMultiCharDelimiter()
    {
        RunPanbyteWithConsoleInput("-f bytes -t bytes -d delimiter", "delimiterdelimiterdelimiter",
            "delimiterdelimiterdelimiter");
        RunPanbyteWithConsoleInput("-f bytes -t hex -d aaa", "testaaatest", "74657374aaa74657374");
        RunPanbyteWithConsoleInput("-f bytes -t hex -d aaa", "aaa", "aaa");
        RunPanbyteWithConsoleInput("-f bytes -t hex -d ttt", "testtttesttttesttttes",
            "746573ttt746573ttt746573ttt746573");
    } */
}