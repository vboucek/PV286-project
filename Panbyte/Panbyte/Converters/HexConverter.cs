using Panbyte.Formats;

namespace Panbyte.Converters;

public class HexConverter : ByteSequenceConverterBase, IConverter
{

    public IFormat InputFormat { get; }
    
    public HexConverter(Hex format)
    {
        InputFormat = format;
    }


    public string ConvertTo(string value, IFormat outputFormat)
    {
        var bytes = Convert.FromHexString(string.Join("", value.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)));

        return BaseConvertTo(bytes, outputFormat);
    }
}