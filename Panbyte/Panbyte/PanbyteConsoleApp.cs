using Panbyte.ArgParsing;
using Panbyte.Converters;
using Panbyte.Formats;

namespace Panbyte;

public class PanbyteConsoleApp : IConsoleApp
{
    private readonly Options _options;
    private readonly IConverter _converter;

    public PanbyteConsoleApp(string[] args)
    {
        try
        {
            var argParser = new ArgParser();
            _options = argParser.ParseArguments(args);
            _converter = ConverterInit(_options.InputFormat);
        }
        catch (Exception e) when (e is FormatException || e is ArgumentException)
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

    public void Start()
    {
        var input = Console.ReadLine();
        Console.WriteLine(_converter.ConvertTo(input!, _options.OutputFormat));
    }
}