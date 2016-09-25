using System;

namespace WebAssist.Optimizations
{
    public static class BundlesManagerExtensions
    {
        public static BundlesManager ForceEnable(this BundlesManager manager)
        {
            BundleSettings.Enabled = true;
            return manager;
        }

        public static BundlesManager ForceDisable(this BundlesManager manager)
        {
            BundleSettings.Enabled = false;
            return manager;
        }

        public static BundlesManager SetConfigurationRoot(this BundlesManager manager, string path)
        {
            BundleSettings.ConfigurationsRootDirectory = path;
            return manager;
        }

        public static BundlesManager AddJsonBundleConfiguration(this BundlesManager manager, params string[] paths)
        {
            if (paths == null)
                throw new ArgumentNullException(nameof(paths));

            foreach (var path in paths)
            {
                string configPath = string.Concat(BundleSettings.ConfigurationsRootDirectory, path);
                var bundleConfig = new JsonBundleConfigFile(configPath);
                manager.Add(bundleConfig);
            }
            
            return manager;
        }

        public static BundlesManager SetJSDistributionPath(this BundlesManager manager, string path)
        {
            BundleSettings.JSDistributionPath = path;
            return manager;
        }

        public static BundlesManager SetCSSDistributionPath(this BundlesManager manager, string path)
        {
            BundleSettings.CSSDistributionPath = path;
            return manager;
        }

        public static BundlesManager UseContentVersioning(this BundlesManager manager, bool useVersioning)
        {
            BundleSettings.UseVersioning = useVersioning;
            return manager;
        }

        public static BundlesManager UseContentVersioning(this BundlesManager manager, bool useVersioning, IVersionResolver versionResolver)
        {
            BundleSettings.UseVersioning = useVersioning;
            BundleSettings.VersioningResolver = versionResolver;
            return manager;
        }
    }
}
