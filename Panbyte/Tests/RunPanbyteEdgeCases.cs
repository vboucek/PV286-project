using System.Diagnostics;
using Panbyte;

namespace Tests;

[TestClass]
public class RunPanbyteEdgeCases : RunPanbyteTest
{
    [TestMethod]
    public void EmptyInput()
    {
        RunPanbyteWithConsoleInput("-f hex -t hex", "", "");
        RunPanbyteWithConsoleInput("-f bytes -t bytes", "", "");
        RunPanbyteWithConsoleInput("-f int -t hex", "", "");
        RunPanbyteWithConsoleInput("-f bits -t bytes", "", "");
    }

    [TestMethod]
    public void TestHelp()
    {
        var output = RunPanbyteWithConsoleInput("-h", "");
        
        Assert.IsTrue(output.Contains("bits") && output.Contains("array") && output.Contains("int") &&
                      output.Contains("bytes") && output.Contains("hex"));
    }
}