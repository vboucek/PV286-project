using Panbyte.Converters;
using Panbyte.Formats;
using Panbyte.OptionsParsing;

namespace Panbyte;

public class PanbyteConsoleApp : IConsoleApp
{
    private readonly PanbyteOptions _options;
    private IConverter? _converter;

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
            ByteArray => throw new NotImplementedException(),
            _ => throw new ArgumentException("Given format is not supported."),
        };

    public PanbyteConsoleApp(string[] args)
    {
        try
        {
            var argParser = new OptionsParser(_formats); // Initialize with supported formats
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
            throw new ArgumentException("Input and output formats cannot be null");
        }

        try
        {
            _converter = ConverterInit(_options.InputFormat);
            var input = Console.ReadLine();
            Console.WriteLine(_converter.ConvertTo(input!, _options.OutputFormat));
        }
        catch (Exception e) when (e is ArgumentException or FormatException or IOException)
        {
            Console.Error.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }
}