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
        var testInputFile = "TestInputFiles/bytes01.txt";
        var testOutputFile = "out.txt";
        var expectedOutputFile = $"ExpectedOutputFiles/bytes01-to-hex.txt";

        File.WriteAllText(testInputFile, UnifyNewLines(File.ReadAllText(testInputFile)));
        File.WriteAllText(expectedOutputFile, UnifyNewLines(File.ReadAllText(expectedOutputFile)));

        var args = new[]
            { "-f", "bytes", "-t", "hex", "-i", testInputFile, "-o", testOutputFile, "-d", Environment.NewLine };

        RunPanbyte(args);

        Assert.AreEqual(File.ReadAllText(expectedOutputFile), File.ReadAllText(testOutputFile));
    }
}