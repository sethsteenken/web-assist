using System.Globalization;
using System.Web;

namespace WebAssist.Optimizations
{
    internal class ContentTag
    {
        public string Value { get; set; }
        public bool IsStatic { get; set; }
        public bool IsBundle { get; set; }

        public ContentTag(string value)
        {
            Value = FormatPath(value);
        }

        private static string FormatPath(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            if(!(value.StartsWith("~") || value.StartsWith("http") || value.StartsWith("ftp") || value.StartsWith(".")))
            {
                if (value.StartsWith("/"))
                    return string.Concat("~", value);
                else
                    return string.Concat("~/", value);
            }

            return value;
        }

        public string Render(string tagFormat)
        {
            if (IsStatic)
                return Value;
            else
                return string.Format(CultureInfo.InvariantCulture, tagFormat, HttpUtility.UrlPathEncode(Value));
        }
    }
}
