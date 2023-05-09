using Panbyte.Formats;
using System.Text.RegularExpressions;
using System.Text;
using Panbyte.OptionsParsing.ArgsParsing;

namespace Panbyte.OptionsParsing;

/// <summary>
/// Obtains options for Panbyte console app.
/// </summary>
public class PanbyteOptionsParser
{
    /// <summary>
    /// Supported mandatory options.
    /// </summary>
    private Dictionary<Switch, Action<Option>> _mandatoryOpts = new();

    /// <summary>
    /// Supported optional options.
    /// </summary>
    private Dictionary<Switch, Action<Option>> _optionalOpts = new();

    /// <summary>
    /// Currently parsed options.
    /// </summary>
    private List<Option> _parsedOpts = new();

    /// <summary>
    /// Formats currently supported by the program.
    /// </summary>
    private readonly ICollection<IFormatModule> _supportedFormats;

    /// <summary>
    /// Currently parsed options.
    /// </summary>
    private PanbyteOptions _options = new();

    public PanbyteOptionsParser(ICollection<IFormatModule> formats)
    {
        _supportedFormats = formats;
        InitializeOpts();
    }

    /// <summary>
    /// Initializes all options with a handler. 
    /// </summary>
    private void InitializeOpts()
    {
        _mandatoryOpts = new()
        {
            {
                new Switch("-f", "--from=", true),
                opt => _options.InputFormat = GetFormat(opt.Argument!)
            },
            {
                new Switch("-t", "--to=", true),
                opt => _options.OutputFormat = GetFormat(opt.Argument!)
            },
            {
                new Switch("-h", "--help", false),
                _ => _options.Help = true
            }
        };

        _optionalOpts = new()
        {
            {
                new Switch("-i", "--input=", true),
                opt => _options.InputFilePath = opt.Argument
            },
            {
                new Switch("-o", "--output=", true),
                opt => _options.OutputFilePath = opt.Argument
            },
            {
                new Switch("-d", "--delimiter=", true),
                opt => _options.Delimiter = ProcessDelimiter(opt.Argument!)
            },
            {
                new Switch(null, "--from-options=", true),
                opt => _options.InputFormat!.ParseInputFormatOption(opt.Argument!)
            },
            {
                new Switch(null, "--to-options=", true),
                opt => _options.OutputFormat!.ParseOutputFormatOption(opt.Argument!)
            }
        };
    }

    /// <summary>
    /// Parses options of a Panbyte application.
    /// </summary>
    /// <param name="args">Console arguments of the programs.</param>
    /// <returns>Initialized PanbyteOptions.</returns>
    /// <exception cref="ArgumentException">when args contain invalid switches or switch misses argument.</exception>
    public PanbyteOptions ParseArguments(string[] args)
    {
        // Combine all opts in one list, to give it to parser
        var allOpts = _mandatoryOpts
            .Select(x => x.Key)
            .Concat(_optionalOpts.Select(x => x.Key))
            .ToList();

        _parsedOpts = new ArgsParser().ParseArgs(args, allOpts);
        _options = new PanbyteOptions();

        ParseMandatoryOptions();

        if (_options.Help)
        {
            return _options;
        }

        ParseOptionalOptions();

        return _options;
    }

    /// <summary>
    /// Gets a new instance of a format by its name.
    /// </summary>
    /// <param name="formatName">Name of the format.</param>
    /// <returns>New format.</returns>
    /// <exception cref="ArgumentException">When format with this name does not exist.</exception>
    private Format GetFormat(string formatName)
    {
        foreach (var format in _supportedFormats)
        {
            if (format.Name.Equals(formatName))
            {
                return format.GetNewFormat();
            }
        }

        throw new ArgumentException($"Invalid format '{formatName}'");
    }

    /// <summary>
    /// Processes the delimiter given at commandline, unescapes entries such as '\xff' because
    /// they are supposed to be interpreted as bytes.
    /// </summary>
    /// <param name="delimiter">string given from commandline supposed to delimit strings</param>
    /// <returns>delimiter converted to bytes</returns>
    private  byte[] ProcessDelimiter(string delimiter)
    {
        string pattern = @"(?<!\\)(\\x[0-9a-f][0-9a-f]|\\x[0-9a-f])";
        var regex = new Regex(pattern);
        
        var result  = regex.Replace(delimiter, match => {
            string hex = match.Value.Substring(2);
            byte b = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            
            return Encoding.Unicode.GetString(new byte[] {b, 0});
        });

        List<byte> output = new List<byte>();
        foreach (var val in result)
        {
            if ((int)val > 255)
                throw new ArgumentException("Delimiter contains invalid characters");
            output.Add((byte)val);
        }
        return output.ToArray();
    }
    
    /// <summary>
    /// Parses mandatory options.
    /// </summary>
    /// <exception cref="ArgumentException">when mandatory options are missing.</exception>
    private void ParseMandatoryOptions()
    {
        foreach (var opt in _parsedOpts.Where(opt => _mandatoryOpts.ContainsKey(opt.Switch)))
        {
            // Call associated option handler
            _mandatoryOpts[opt.Switch](opt);
        }

        if (_options.Help)
        {
            return;
        }

        if (_options.InputFormat is null)
        {
            throw new ArgumentException("Missing input format option.");
        }

        if (_options.OutputFormat is null)
        {
            throw new ArgumentException("Missing output format option.");
        }
    }

    /// <summary>
    /// Parses optional arguments.
    /// </summary>
    private void ParseOptionalOptions()
    {
        foreach (var opt in _parsedOpts.Where(opt => _optionalOpts.ContainsKey(opt.Switch)))
        {
            // Call associated option handler
            _optionalOpts[opt.Switch](opt);
        }

        // Delimiter was not set, use format's default options
        _options.Delimiter ??= _options.InputFormat?.DefaultDelimiter;
    }
}