using System.Text;
using Panbyte.Formats;
using Panbyte.Utils;

namespace Panbyte.Converters.AuxiliaryObjects;

public class Array : ArrayContentItem
{
    public List<ArrayContentItem> Content { get; }

    public Array(List<ArrayContentItem> content)
    {
        Content = content;
    }

    public string ArrayContentToString(ByteArray outputFormat)
    {
        var openingBracket = ByteArrayUtils.GetOpeningBracket(outputFormat.Brackets);
        var closingBracket = ByteArrayUtils.GetClosingBracket(outputFormat.Brackets);

        var result = new StringBuilder(openingBracket);
        
        foreach (var item in Content)
        {
            if (item is Byte)
            {
                var byteItem = (Byte) item;
                result.Append(byteItem.Content.ToString());
            }
            else
            {
                var arrayItem = (Array)item;
                arrayItem.ArrayContentToString(outputFormat);
            }

            result.Append(", ");
        }

        result.Length -= 2;

        result.Append(closingBracket);
        return result.ToString();
    }
}