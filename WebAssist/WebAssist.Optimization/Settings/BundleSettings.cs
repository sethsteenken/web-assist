using System.Collections.Generic;
using System.Web;

namespace WebAssist.Optimizations
{
    public static class BundleSettings
    {
        private static readonly string _defaultJSDistPath = @"/content/js/";
        private static readonly string _defaultCSSDistPath = @"/content/css/";

        private static string _jsDistPath;
        private static string _cssDistPath;

        private static bool _enableOptimizations = true;
        private static bool _enableOptimizationsSet = false;

        private static IVersionResolver _versioningResolver;

        private static IList<BundleConfigFile> _bundleConfigs;

        public static BundlesManager Setup()
        {
            return new BundlesManager();
        }

        internal static IList<BundleConfigFile> BundleConfigurations
        {
            get
            {
                if (_bundleConfigs == null)
                    _bundleConfigs = new List<BundleConfigFile>();
                return _bundleConfigs;
            }
        }

        internal static bool Enabled
        {
            get
            {
                if (!_enableOptimizationsSet && HttpContext.Current != null)
                    return !HttpContext.Current.IsDebuggingEnabled;
                return _enableOptimizations;
            }
            set
            {
                _enableOptimizations = value;
                _enableOptimizationsSet = true;
            }
        }

        internal static string JSDistributionPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_jsDistPath))
                    return _defaultJSDistPath;
                return _jsDistPath;
            }
            set { _jsDistPath = value; }
        }

        internal static string CSSDistributionPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_cssDistPath))
                    return _defaultCSSDistPath;
                return _cssDistPath;
            }
            set { _cssDistPath = value; }
        }

        internal static bool UseVersioning { get; set; }

        internal static IVersionResolver VersioningResolver
        {
            get
            {
                if (_versioningResolver == null)
                    _versioningResolver = new FileWriteTimeVersionResolver();
                return _versioningResolver;
            }
            set { _versioningResolver = value; }
        }

        internal static string ConfigurationsRootDirectory { get; set; }
    }
}
