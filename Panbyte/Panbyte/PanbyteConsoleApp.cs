using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.InputProcessing;
using Panbyte.OptionsParsing;

namespace Panbyte;

/// <summary>
/// Panbyte - console app for conversions between various representations of byte sequences.
/// </summary>
public class PanbyteConsoleApp : IConsoleApp
{
    private readonly PanbyteOptions _options;

    private readonly List<IFormatModule> _formats = new()
    {        
        new FormatModule<Bytes>("bytes", "Raw bytes"),
        new FormatModule<Hex>("hex", "Hex-encoded string"),
        new FormatModule<Int>("int", "Integer"),
        new FormatModule<Bits>("bits", "0,1-represented bits"),
        new FormatModule<ByteArray>("array", "Byte array"),
    };

    private static IConverter ConverterInit(Format inputFormat) =>
        inputFormat switch
        {
            Bytes b => new BytesConverter(b),
            Hex h => new HexConverter(h),
            Bits b => new BitsConverter(b),
            Int i => new IntConverter(i),
            ByteArray a => new ArrayConverter(a),
            _ => throw new ArgumentException("Given format is not supported."),
        };

    public PanbyteConsoleApp(string[] args)
    {
        try
        {
            var argParser = new PanbyteOptionsParser(_formats); // Initialize with supported formats
            _options = argParser.ParseArguments(args);
        }
        catch (ArgumentException e)
        {
            Console.Error.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }

    private void PrintHelp()
    {
        Console.WriteLine(@"
./panbyte [ARGS...]

ARGS:
-f FORMAT      --from=FORMAT            Set input data format
               --from-options=OPTIONS   Set input options
-t FORMAT      --to=FORMAT              Set output data format
               --to-options=OPTIONS     Set output options
-i FILE        --input=FILE             Set input file (default stdin)
-o FILE        --output=FILE            Set output file (default stdout)
-d DELIMITER   --delimiter=DELIMITER    Record delimiter (default newline)
-h             --help                   Print help

FORMATS:");
        foreach (var format in _formats)
        {
            Console.WriteLine(format);   
        }
    }

    public void Start()
    {
        if (_options.Help)
        {
            PrintHelp();
            return;
        }

        if (_options.InputFormat is null || _options.OutputFormat is null)
        {
            Console.Error.WriteLine("Formats cannot be null.");
            Environment.Exit(1);
        }

        try
        {
            // Initialize converted based on given input format
            var converter = ConverterInit(_options.InputFormat);

            // Initialize input processor with converter and output format 
            var processor = new InputProcessor(converter, _options.OutputFormat);

            // Process given input
            processor.ProcessInput(_options.Delimiter, _options.InputFilePath, _options.OutputFilePath);
        }
        catch (OutOfMemoryException)
        { 
            Console.Error.WriteLine("The application has run out of memory. Try to provide smaller input.");
            Environment.Exit(1);
        }
        catch (Exception e) when (e is ArgumentException or FormatException or IOException)
        {
            Console.Error.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }
}