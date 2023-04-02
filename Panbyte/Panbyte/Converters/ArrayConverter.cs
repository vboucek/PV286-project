using Panbyte.Formats;

namespace Panbyte.Converters;

public class ArrayConverter : IConverter
{
    public IFormat InputFormat { get; }
    public string ConvertTo(string value, IFormat outputFormat)
    {
        throw new NotImplementedException();
    }
}