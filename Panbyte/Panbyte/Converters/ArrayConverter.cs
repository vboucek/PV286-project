using Panbyte.Converters.AuxiliaryObjects;
using Panbyte.Formats;
using Array = System.Array;

namespace Panbyte.Converters;

public class ArrayConverter : IConverter
{
    public Format InputFormat { get; }

    private List<ArrayItem> content = new();

    public string ConvertTo(string value, Format outputFormat)
    {
        throw new NotImplementedException();
    }
}