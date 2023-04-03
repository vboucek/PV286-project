using Panbyte.ArgParsing;
using Panbyte.Converters;
using Panbyte.Formats;

namespace Panbyte;

/// <summary>
/// Panbyte - console app for conversions between various representations of byte sequences.
/// </summary>
public class PanbyteConsoleApp : IConsoleApp
{
    private readonly IOptions _options;
    private IConverter? _converter;

    public PanbyteConsoleApp(string[] args)
    {
        try
        {
            var argParser = new ArgParser();
            _options = argParser.ParseArguments(args);
        }
        catch (ArgumentException e)
        {
            Console.Error.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }

    private IConverter ConverterInit(IFormat inputFormat) =>
        inputFormat switch
        {
            Bytes b => new BytesConverter(b),
            Hex h => new HexConverter(h),
            Bits b => new BitsConverter(b),
            Int i => new IntConverter(i),
            ByteArray => throw new NotImplementedException(),
            _ => throw new ArgumentException("Invalid format"),
        };

    private static void PrintHelp()
    {
        Console.WriteLine(@"
./panbyte [ARGS...]

ARGS:
-f FORMAT     --from=FORMAT           Set input data format
              --from-options=OPTIONS  Set input options
-t FORMAT     --to=FORMAT             Set output data format
              --to-options=OPTIONS    Set output options
-i FILE       --input=FILE            Set input file (default stdin)
-o FILE       --output=FILE           Set output file (default stdout)
-d DELIMITER  --delimiter=DELIMITER   Record delimiter (default newline)
-h            --help                  Print help

FORMATS:
bytes                                 Raw bytes
hex                                   Hex-encoded string
int                                   Integer
bits                                  0,1-represented bits
array                                 Byte array
");
    }
    
    public void Start()
    {
        if (_options.Help)
        {
            PrintHelp();
        }

        if (_options is HelpOnlyOptions)
        {
            return;
        }
        
        try
        {
            _converter = ConverterInit(((FullOptions)_options).InputFormat);
            var input = Console.ReadLine();
            Console.WriteLine(_converter.ConvertTo(input!, ((FullOptions)_options).OutputFormat));
        }
        catch (Exception e) when (e is ArgumentException or FormatException or IOException)
        {
            Console.Error.WriteLine(e.Message);
            Environment.Exit(1);
        }
    }
}