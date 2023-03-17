using Panbyte;

namespace Tests;

public abstract class RunPanbyteTest
{
    protected string RunPanbyte(string consoleInput, string[] args)
    {
        // Mock console input with StringReader and redirect console output to StringWriter
        using var sw = new StringWriter();
        using var sr = new StringReader(consoleInput);
        Console.SetIn(sr);
        Console.SetOut(sw);

        // Run the app
        IConsoleApp app = new PanbyteConsoleApp(args);
        app.Start();

        // Return console result
        return sw.ToString();
    }
}