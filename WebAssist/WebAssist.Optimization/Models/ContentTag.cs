using System.Globalization;
using System.Web;

namespace WebAssist.Optimization
{
    public class ContentTag
    {
        public ContentTag(string value) : this (value, false)
        {

        }

        public ContentTag(string value, bool isBundle) : this (value, isBundle, false)
        {

        }

        public ContentTag(string value, bool isBundle, bool isStatic)
        {
            Value = FormatPath(value);
            IsBundle = isBundle;
            IsStatic = IsStatic;
        }

        public string Value { get; private set; }
        public bool IsStatic { get; private set; }
        public bool IsBundle { get; private set; }

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

        public void UpdateValue(string value)
        {
            Value = value;
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
