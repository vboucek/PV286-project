using System.Diagnostics;
using Panbyte;

namespace Tests;

public abstract class RunPanbyteTest
{
    protected void RunPanbyteWithConsoleInput(string args, string input, string expectedOutput)
    {
        // Run the app in a new process and get the input
        var process = new Process();
        process.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory() ,"Panbyte");
        process.StartInfo.Arguments = args;
        
        process.StartInfo.UseShellExecute = false;
        
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.Start();

        var inputStream = process.StandardInput;
        
        inputStream.Write(input);
        inputStream.Close();

        var outputStream = process.StandardOutput;

        Assert.AreEqual(expectedOutput, outputStream.ReadToEnd());

        process.WaitForExit();
        Assert.AreEqual(process.ExitCode, 0);
    }
    
    protected void RunPanbyte(string[] args)
    {
        IConsoleApp app = new PanbyteConsoleApp(args);
        app.Start();
    }
}