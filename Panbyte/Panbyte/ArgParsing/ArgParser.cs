using Panbyte.Formats;
using Panbyte.Formats.Enums;

namespace Panbyte.ArgParsing;

/// <summary>
/// Argument parser for Panbyte console app.
/// </summary>
public class ArgParser
{
    // remaining arguments
    private List<string> _argsToParse = new();

    /// <summary>
    /// Parses Panbyte options from the array of strings. Returns initialized FullOptions or HelpOnlyOptions.
    /// </summary>
    /// <param name="args">Array of strings received from the main function.</param>
    /// <returns>Parsed options.</returns>
    public IOptions ParseArguments(string[] args)
    {
        _argsToParse = new List<string>(args);
        var newOptions = ParseMandatoryOptions();

        if (newOptions.GetType() == typeof(HelpOnlyOptions))
        {
            return newOptions;
        }

        ParseOptionalOptions((FullOptions)newOptions);
        return newOptions;
    }

    /// <summary>
    /// Parses mandatory options required by Panbyte console app. This means "help" option or "from" and "to" options
    /// must be specified. Otherwise, exception is thrown.
    /// </summary>
    /// <returns>New options object, initialized with mandatory fields.</returns>
    /// <exception cref="ArgumentException">thrown when invalid options are given.</exception>
    private IOptions ParseMandatoryOptions()
    {
        IFormat? inputFormat = null;
        IFormat? outputFormat = null;
        bool help = false;

        int i = 0;

        List<string> remainingArgs = new();

        while (i < _argsToParse.Count)
        {
            switch (_argsToParse[i])
            {
                // From format option
                case "-f":
                    inputFormat = ParseFormat(GetOptionArg(i, "-f"));
                    i++;
                    break;

                case var s when s.StartsWith("--from="):
                    inputFormat = ParseFormat(s[(s.IndexOf('=') + 1)..]); // takes everything behind "="
                    break;

                // To format option
                case "-t":
                    outputFormat = ParseFormat(GetOptionArg(i, "-t"));
                    i++;
                    break;

                case var s when s.StartsWith("--to="):
                    outputFormat = ParseFormat(s[(s.IndexOf('=') + 1)..]);
                    break;

                // Help option
                case "-h" or "--help":
                    help = true;
                    break;

                // Non-mandatory argument, add in remaining arguments list
                default:
                    remainingArgs.Add(_argsToParse[i]);
                    break;
            }

            i++;
        }

        if (help && inputFormat is null && outputFormat is null)
        {
            return new HelpOnlyOptions();
        }

        if (inputFormat is null || outputFormat is null)
        {
            // Input or output format is not specified -> error
            throw new ArgumentException("Missing format options");
        }

        _argsToParse = remainingArgs;
        return new FullOptions(inputFormat, outputFormat, help);
    }

    /// <summary>
    /// Parses optional arguments.
    /// </summary>
    /// <param name="fullOptions">FullOptions object with initialized input and output format options.</param>
    /// <exception cref="ArgumentException">is thrown when invalid options are found.</exception>
    private void ParseOptionalOptions(FullOptions fullOptions)
    {
        int i = 0;

        while (i < _argsToParse.Count)
        {
            switch (_argsToParse[i])
            {
                // Input file option
                case "-i":
                    fullOptions.InputFilePath = GetOptionArg(i, "-i");
                    i++;
                    break;

                case var s when s.StartsWith("--input="):
                    fullOptions.InputFilePath = s[(s.IndexOf('=') + 1)..];
                    break;

                // Output file option
                case "-o":
                    fullOptions.OutputFilePath = GetOptionArg(i, "-o");
                    i++;
                    break;

                case var s when s.StartsWith("--output="):
                    fullOptions.OutputFilePath = s[(s.IndexOf('=') + 1)..];
                    break;

                // Delimiter option
                case "-d":
                    fullOptions.Delimiter = GetOptionArg(i, "-d");
                    i++;
                    break;

                case var s when s.StartsWith("--delimiter="):
                    fullOptions.Delimiter = s[(s.IndexOf('=') + 1)..];
                    break;

                // From format options 
                case var s when s.StartsWith("--from-options="):
                    ParseInputFormatOption(fullOptions.InputFormat, s[(s.IndexOf('=') + 1)..]);
                    break;

                // Output format options 
                case var s when s.StartsWith("--to-options="):
                    ParseOutputFormatOption(fullOptions.OutputFormat, s[(s.IndexOf('=') + 1)..]);
                    break;

                // Found invalid option
                default:
                    throw new ArgumentException($"Invalid argument '{_argsToParse[i]}'");
            }

            i++;
        }
    }

    /// <summary>
    /// Initializes format object based on its string name.
    /// </summary>
    /// <param name="format">Lowercase format name.</param>
    /// <returns>New format object</returns>
    /// <exception cref="ArgumentException">when invalid format is given.</exception>
    private IFormat ParseFormat(string format) =>
        format switch
        {
            "bytes" => new Bytes(),
            "hex" => new Hex(),
            "int" => new Int(),
            "bits" => new Bits(),
            "array" => new ByteArray(),
            _ => throw new ArgumentException($"Invalid format type '{format}'"),
        };

    /// <summary>
    /// Parses argument of a option.
    /// </summary>
    /// <param name="currentIndex">Current index in options array.</param>
    /// <param name="optionName">Name of currently parsed option.</param>
    /// <returns>Parsed argument.</returns>
    /// <exception cref="ArgumentException">index reaches options array boundaries.</exception>
    private string GetOptionArg(int currentIndex, string optionName)
    {
        if (currentIndex + 1 >= _argsToParse.Count)
        {
            throw new ArgumentException($"'{optionName}' option requires a argument");
        }

        return _argsToParse[currentIndex + 1];
    }

    /// <summary>
    /// Parses input format options.
    /// </summary>
    /// <param name="inputFormat">Input format object.</param>
    /// <param name="option">Textual representation of a input format option.</param>
    /// <exception cref="ArgumentException">when invalid format options is given.</exception>
    private void ParseInputFormatOption(IFormat inputFormat, string option)
    {
        switch (inputFormat)
        {
            case Int integer:
                integer.Endianness = option switch
                {
                    "big" => Endianness.BigEndian,
                    "little" => Endianness.LittleEndian,
                    _ => throw new ArgumentException(
                        $"Invalid input format option '{option}' for format '{integer}'"),
                };
                break;

            case Bits bits:
                bits.BitPadding = option switch
                {
                    "left" => BitPadding.Left,
                    "right" => BitPadding.Right,
                    _ => throw new ArgumentException(
                        $"Invalid input format option '{option}' for format '{bits}'"),
                };
                break;

            default:
                throw new ArgumentException(
                    $"Invalid input format option '{option}' for format '{inputFormat}'");
        }
    }

    /// <summary>
    /// Parses output format options.
    /// </summary>
    /// <param name="outputFormat">Output format object.</param>
    /// <param name="option">Textual representation of a output format option.</param>
    /// <exception cref="ArgumentException">when invalid format options is given.</exception>
    private void ParseOutputFormatOption(IFormat outputFormat, string option)
    {
        switch (outputFormat)
        {
            case Int integer:
                integer.Endianness = option switch
                {
                    "big" => Endianness.BigEndian,
                    "little" => Endianness.LittleEndian,
                    _ => throw new ArgumentException(
                        $"Invalid output format option '{option}' for format '{integer}'"),
                };
                break;

            case ByteArray array:
                switch (option)
                {
                    case "0x":
                        array.ArrayFormat = ArrayFormat.Hex;
                        break;

                    case "0":
                        array.ArrayFormat = ArrayFormat.Decimal;
                        break;

                    case "a":
                        array.ArrayFormat = ArrayFormat.Char;
                        break;

                    case "0b":
                        array.ArrayFormat = ArrayFormat.Binary;
                        break;

                    case "{" or "}" or "{}":
                        array.Brackets = Brackets.Curly;
                        break;

                    case "(" or ")" or "()":
                        array.Brackets = Brackets.Regular;
                        break;

                    case "[" or "]" or "[]":
                        array.Brackets = Brackets.Square;
                        break;

                    default:
                        throw new ArgumentException(
                            $"Invalid output format option '{option}' for format '{outputFormat}'");
                }

                break;

            default:
                throw new ArgumentException(
                    $"Invalid output format option '{option}' for format '{outputFormat}");
        }
    }
}