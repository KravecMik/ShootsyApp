using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Shootsy.Security
{
    public static class SupportMethods
    {
        public static string RegexStringConverter(this string str)
        {
            Regex regex = new Regex(@"\\u([a-f0-9]{4})", RegexOptions.IgnoreCase);
            var result = regex.Replace(str, match => ((Char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
            return result;
        }

        public static List<string> RegexStringListConverter(this List<string> str)
        {
            var result = str.Select(x => RegexStringConverter(x)).ToList();
            return result;
        }

        public static IEnumerable<string> GetPublicProperties(this Type type) => type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
            .Where(x => x.SetMethod.IsPublic)
            .Select(x => x.Name);

        public static bool EqualsPath(this string path, string propertyName, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase) =>
            path.Equals($"/{propertyName}", stringComparison);
    }
}