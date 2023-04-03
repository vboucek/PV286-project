namespace Tests;

[TestClass]
public class FileInputOutputTest : RunPanbyteTest
{
    [TestMethod]
    [DeploymentItem(@"TestInputFiles/bytes01.txt")]
    [DeploymentItem(@"ExpectedOutputFiles/bytes01-to-hex.txt")]
    public void ConvertBytesInput()
    {
        var testInputFile = "TestInputFiles/bytes01.txt";
        var testOutputFile = "out.txt";
        var expectedOutputFile = $"ExpectedOutputFiles/bytes01-to-hex.txt";
        
        var args = new[] { "-f", "bytes", "-t", "hex", "-i", testInputFile, "-o", testOutputFile, "-d", "\n" };
        
        RunPanbyte(args);    
        
        Assert.AreEqual(File.ReadAllText(expectedOutputFile), File.ReadAllText(testOutputFile));
    }
}