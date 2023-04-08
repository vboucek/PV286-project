using System.Diagnostics;
using Panbyte;

namespace Tests;

[TestClass]
public class RunPanbyteErrorCases : RunPanbyteTest
{
    [TestMethod]
    public void TestErrorInput()
    {
        RunPanbyteExpectFail("-f hex -t int", "somerandom");
        RunPanbyteExpectFail("-f int -t int", "somerandom");
        RunPanbyteExpectFail("-f bits -t int", "5521311313");
    }
    
    [TestMethod]
    public void TestErrorArgs()
    {
        RunPanbyteExpectFail("-f -t int", "somerandom");
        RunPanbyteExpectFail("-f invalid -t int", "somerandom");
        RunPanbyteExpectFail("", "somerandom");
    }
}