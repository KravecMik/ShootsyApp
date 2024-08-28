using System.Globalization;
using System.Text.RegularExpressions;

namespace Shootsy.Security
{
    public class SupportMethods
    {
        public SupportMethods()
        {
        }

        public string RegexStringConverter(string str)
        {
            Regex regex = new Regex(@"\\u([a-f0-9]{4})", RegexOptions.IgnoreCase);
            var result = regex.Replace(str, match => ((Char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
            return result;
        }

        public List<string> RegexStringListConverter(List<string> str)
        {
            var result = str.Select(x => RegexStringConverter(x)).ToList();
            return result;
        }
    }
}
