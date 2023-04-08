namespace Tests;

[TestClass]
public class FileInputOutputTest : RunPanbyteTest
{
    private static string UnifyNewLines(string value)
    {
        return value
            .Replace("\r\n", "\n")
            .Replace("\n", Environment.NewLine);
    }

    [TestMethod]
    [DeploymentItem(@"ExpectedOutputFiles/bytes01-to-hex.txt")]
    public void ConvertBytesInput()
    {
        var testInputFile = Path.Combine("TestInputFiles", "bytes01.txt");
        var testOutputFile = "out.txt";
        var expectedOutputFile = Path.Combine("ExpectedOutputFiles", "bytes01-to-hex.txt");
        
        var args = new[]
            { "-f", "bytes", "-t", "hex", "-i", testInputFile, "-o", testOutputFile, "-d", Environment.NewLine };

        RunPanbyte(args);

        Assert.AreEqual(UnifyNewLines(File.ReadAllText(expectedOutputFile)), UnifyNewLines(File.ReadAllText(testOutputFile)));
    }
}