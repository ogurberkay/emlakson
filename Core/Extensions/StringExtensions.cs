using System.Text;

namespace Core.Extensions;

public static class StringExtensions
{
    public static string TurkishChrToEnglishChr(this string text)
    {
        if (string.IsNullOrEmpty(text)) return text;

        Dictionary<char, char> TurkishChToEnglishChDic = new Dictionary<char, char>()
        {
            {'ç','c'},
            {'Ç','C'},
            {'ğ','g'},
            {'Ğ','G'},
            {'ı','i'},
            {'İ','I'},
            {'ş','s'},
            {'Ş','S'},
            {'ö','o'},
            {'Ö','O'},
            {'ü','u'},
            {'Ü','U'}
        };

        return text.Aggregate(new StringBuilder(), (sb, chr) =>
        {
            if (TurkishChToEnglishChDic.ContainsKey(chr))
                sb.Append(TurkishChToEnglishChDic[chr]);
            else
                sb.Append(chr);

            return sb;
        }).ToString();
    }
}