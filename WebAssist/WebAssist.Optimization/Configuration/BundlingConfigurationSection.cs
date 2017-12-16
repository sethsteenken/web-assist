using System.Configuration;
using System.Web;

namespace WebAssist.Optimizations
{
    internal class BundlingConfigurationSection : ConfigurationSection, IBundlingSettings
    {
        public const string SectionName = "webBundling";

        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get
            {
                object setting = this["enabled"];
                return setting == null ? !HttpContext.Current.IsDebuggingEnabled : (bool)setting;
            }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("jsOutputDirectory")]
        public string JSOutputDirectory
        {
            get
            {
                string value = (string)this["jsOutputDirectory"];
                if (string.IsNullOrWhiteSpace(value))
                    value = @"/content/js/";
                return value;
            }
            set { this["jsOutputDirectory"] = value; }
        }

        [ConfigurationProperty("cssOutputDirectory")]
        public string CSSOutputDirectory
        {
            get
            {
                string value = (string)this["cssOutputDirectory"];
                if (string.IsNullOrWhiteSpace(value))
                    value = @"/content/css/";
                return value;
            }
            set { this["cssOutputDirectory"] = value; }
        }

        [ConfigurationProperty("useVersioning")]
        public bool UseVersioning
        {
            get
            {
                return (bool)this["useVersioning"];
            }
            set { this["useVersioning"] = value; }
        }

        [ConfigurationProperty("configurationsDirectory")]
        public string ConfigurationsDirectory
        {
            get
            {
                string value = (string)this["configurationsDirectory"];
                if (string.IsNullOrWhiteSpace(value))
                    value = @"/Config/BundleDefinitions/";
                return value;
            }
            set { this["configurationsDirectory"] = value; }
        }
    }
}
