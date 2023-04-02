using Panbyte.Converters.AuxiliaryObjects;
using Panbyte.Formats;
using Array = System.Array;

namespace Panbyte.Converters;

public class ArrayConverter : IConverter
{
    public IFormat InputFormat { get; }

    private List<ArrayItem> content = new();

    public string ConvertTo(string value, IFormat outputFormat)
    {
        throw new NotImplementedException();
    }
}