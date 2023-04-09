using System.Diagnostics;
using Panbyte;

namespace Tests;

public abstract class RunPanbyteTest
{
    /// <summary>
    /// Runs whole app in another process and checks it stdout and exit code.
    /// </summary>
    /// <param name="args">Program args.</param>
    /// <param name="input">Program stdin.</param>
    /// <param name="expectedOutput">Expected program stdout. If null is given, output is not checked.</param>
    /// <returns>Actual program output.</returns>
    protected string RunPanbyteWithConsoleInput(string args, string input, string? expectedOutput = null)
    {
        // Run the app in a new process and get the input
        var process = new Process();
        
        RunProcess(process, args);
        
        var inputStream = process.StandardInput;

        inputStream.Write(input);
        inputStream.Close();

        var outputStream = process.StandardOutput;

        var output = outputStream.ReadToEnd();

        if (expectedOutput is not null)
        {
            Assert.AreEqual(expectedOutput, output);
        }

        process.WaitForExit();
        Assert.AreEqual(0, process.ExitCode);

        return output;
    }

    protected string RunPanbyteExpectFail(string args, string input)
    {
        var process = new Process();
        
        RunProcess(process, args);
        
        var inputStream = process.StandardInput;

        inputStream.Write(input);
        inputStream.Close();

        var outputStream = process.StandardError;

        var output = outputStream.ReadToEnd();

        process.WaitForExit();
        Assert.AreEqual(1, process.ExitCode);

        return output;
    }

    protected void RunPanbyte(string[] args)
    {
        IConsoleApp app = new PanbyteConsoleApp(args);
        app.Start();
    }

    private void RunProcess(Process process, string args)
    {
        process.StartInfo.FileName = Path.Combine(Directory.GetCurrentDirectory(), "Panbyte");
        process.StartInfo.Arguments = args;

        process.StartInfo.UseShellExecute = false;

        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.Start();
    }
}