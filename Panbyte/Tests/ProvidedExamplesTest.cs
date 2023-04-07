namespace Tests;

[TestClass]
public class ProvidedExamplesTest : RunPanbyteTest
{
    [TestMethod]
    public void BytesTest()
    {
        RunPanbyteWithConsoleInput("-f bytes -t bytes", "test", "test");
    }

    [TestMethod]
    public void HexTest()
    {
        RunPanbyteWithConsoleInput("-f hex -t bytes", "74657374", "test");
        RunPanbyteWithConsoleInput("-f bytes -t hex", "test", "74657374");
        RunPanbyteWithConsoleInput("-f hex -t bytes", "74 65 73 74", "test");
    }

    [TestMethod]
    public void IntTest()
    {
        RunPanbyteWithConsoleInput("-f int -t hex", "1234567890", "499602d2");
        RunPanbyteWithConsoleInput("-f int --from-options=big -t hex", "1234567890", "499602d2");
        RunPanbyteWithConsoleInput("-f int --from-options=little -t hex", "1234567890", "d2029649");
        RunPanbyteWithConsoleInput("-f hex -t int", "499602d2", "1234567890");
        RunPanbyteWithConsoleInput("-f hex -t int --to-options=big", "499602d2", "1234567890");
        RunPanbyteWithConsoleInput("-f hex -t int --to-options=little", "d2029649", "1234567890");
    }

    [TestMethod]
    public void BitsTest()
    {
        RunPanbyteWithConsoleInput("-f bits -t bytes", "100 1111 0100 1011", "OK");
        RunPanbyteWithConsoleInput("-f bits --from-options=left -t bytes", "100111101001011", "OK");
        RunPanbyteWithConsoleInput("-f bits --from-options=right -t hex", "100111101001011", "9e96");
        RunPanbyteWithConsoleInput("-f bytes -t bits", "OK", "0100111101001011");
    }

    // to be implemented

    [TestMethod]
    public void ByteArrayTest()
    {
        /*RunPanbyteWithConsoleInput("-f hex -t array", "01020304", "{0x1, 0x2, 0x3, 0x4}");
        RunPanbyteWithConsoleInput("-f array -t hex", @"{0x01, 2, 0b11, '\x04'}", "01020304");
        RunPanbyteWithConsoleInput("-f array -t array", @"{0x01,2,0b11 ,'\x04' }", "{0x1, 0x2, 0x3, 0x4}");
        RunPanbyteWithConsoleInput("-f array -t array --to-options=0x", @"[0x01, 2, 0b11, '\x04']", "{0x1, 0x2, 0x3, 0x4}");
        RunPanbyteWithConsoleInput("-f array -t array --to-options=0", @"(0x01, 2, 0b11, '\x04')", "{1, 2, 3, 4}");
        RunPanbyteWithConsoleInput("-f array -t array --to-options=a", @"{0x01, 2, 0b11, '\x04'}", @"{'\x01', '\x02', '\x03', '\x04'}");
        RunPanbyteWithConsoleInput("-f array -t array --to-options=0b", @"[0x01, 2, 0b11, '\x04']", @"{0b1, 0b10, 0b11, 0b100}");
        RunPanbyteWithConsoleInput("-f array -t array --to-options=(", @"(0x01, 2, 0b11, '\x04')", "(0x1, 0x2, 0x3, 0x4)");
        RunPanbyteWithConsoleInput("-f array -t array --to-options=0 --to-options=[", @"{0x01, 2, 0b11, '\x04'}", "[1, 2, 3, 4]");
        RunPanbyteWithConsoleInput("-f array -t array", "[[1, 2], [3, 4], [5, 6]]", "{{0x1, 0x2}, {0x3, 0x4}, {0x5, 0x6}}");
        RunPanbyteWithConsoleInput("-f array -t array --to-options={ --to-options=0", "[[1, 2], [3, 4], [5, 6]]", "{{1, 2}, {3, 4}, {5, 6}}");
        RunPanbyteWithConsoleInput("-f array -t array --to-options=0 --to-options=[", @"{{0x01, (2), [3, 0b100, 0x05], '\x06'}}", "[[1, [2], [3, 4, 5], 6]]");
        RunPanbyteWithConsoleInput("-f array -t array", @"()", "{}");
        RunPanbyteWithConsoleInput("-f array -t array --to-options=[", "([],{})", "[[], []]"); */
    }
}