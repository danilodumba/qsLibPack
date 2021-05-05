using System.Linq;
using System.Text.RegularExpressions;

namespace qsLibPack.Extensions
{
    public static class StringExtension
    {
        public static string OnlyNumbers(this string field)
        {
            return Regex.Replace(field, "[^0-9]", string.Empty);
        }
    }
}