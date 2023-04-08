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

    public byte[] ArrayContentToByteArray(ByteArray outputFormat)
    {
        var openingBracket = ByteArrayUtils.GetOpeningBracket(outputFormat.Brackets);
        var closingBracket = ByteArrayUtils.GetClosingBracket(outputFormat.Brackets);

        var result = new List<byte> { openingBracket };
        if (Content.Count == 0)
        {
            result.Add(closingBracket);
            return result.ToArray();
        }

        foreach (var item in Content)
        {
            if (item is Byte byteItem)
            {
                var byteString = ByteUtils.ConvertToString(byteItem.Content, outputFormat.ArrayFormat);
                var byteBytes = Encoding.ASCII.GetBytes(byteString);
                result.AddRange(byteBytes);
            }
            else
            {
                var arrayItem = (Array) item;
                result.AddRange(arrayItem.ArrayContentToByteArray(outputFormat));
            }
            
            result.AddRange(new List<byte> { Convert.ToByte(','), Convert.ToByte(' ') });
        }

        result.RemoveRange(result.Count - 2, 2);

        result.Add(closingBracket);
        return result.ToArray();
    }
}