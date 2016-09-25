using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;

namespace WebAssist.Optimizations
{
    internal class BundleResolver : IBundleResolver
    {
        private readonly IVersionResolver versioningResolver;

        public BundleResolver(IVersionResolver versioningResolver)
        {
            //check for all the .json last modified date and update the bundle configuration date if different
            CheckBundleConfigFiles();
            this.versioningResolver = versioningResolver;
        }

        private void CheckBundleConfigFiles()
        {
            if (!BundleConfigurations.Any())
                return;

            foreach (var configFile in BundleConfigurations)
            {
                var lastModifiedDate = File.GetLastWriteTime(configFile.Path);
                if (lastModifiedDate != configFile.LastUpdatedDate)
                {
                    configFile.UpdateContentFromConfiguration();
                    configFile.LastUpdatedDate = lastModifiedDate;
                }
            }
        }

        private IList<BundleConfigFile> BundleConfigurations
        {
            get { return BundleSettings.BundleConfigurations; }
        }

        public IEnumerable<string> GetBundleContents(string name)
        {
            var bundle = FindBundle(name);
            if (bundle != null)
                return bundle.Files;

            return new List<string>();
        }

        public string GetBundleUrl(Bundle bundle)
        {
            if (bundle == null)
                return string.Empty;

            var virtualPath = GetBundleFullPath(bundle);

            if (BundleSettings.UseVersioning)
                virtualPath = versioningResolver.GetVersionedPath(virtualPath);

            return virtualPath;
        }

        public bool TryGetBundle(string name, out Bundle bundle)
        {
            bundle = FindBundle(name);
            return bundle != null;
        }

        private Bundle FindBundle(string pathOrName)
        {
            foreach (var config in BundleConfigurations)
            {
                var bundle = config.Content.Scripts.Where(x => string.Compare(x.Name, pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                                            string.Compare(x.Filename, pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                                            string.Compare(string.Concat(BundleSettings.JSDistributionPath, x.SubPath, x.Filename), pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0)
                                            .FirstOrDefault();

                if (bundle == null)
                {
                    bundle = config.Content.Styles.Where(x => string.Compare(x.Name, pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                                            string.Compare(x.Filename, pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                                            string.Compare(string.Concat(BundleSettings.CSSDistributionPath, x.SubPath, x.Filename), pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0)
                                            .FirstOrDefault();

                    if (bundle != null)
                        bundle.Type = BundleType.CSS;
                }
                else
                    bundle.Type = BundleType.JS;

                if (bundle != null)
                {
                    return bundle;
                }
            }

            return null;
        }

        private string GetBundleFullPath(Bundle bundle)
        {   
            switch (bundle.Type)
            {
                case BundleType.JS:
                    return BuildFullPath(BundleSettings.JSDistributionPath, bundle);
                case BundleType.CSS:
                    return BuildFullPath(BundleSettings.CSSDistributionPath, bundle);
                default:
                    return string.Empty;
            }
        }

        private static string BuildFullPath(string distPath, Bundle bundle)
        {
            if (bundle == null)
                return string.Empty;

            var hasSubPath = !string.IsNullOrWhiteSpace(bundle.SubPath);
            return string.Concat(distPath, hasSubPath ? (string.Concat(bundle.SubPath, bundle.SubPath.EndsWith("/") ? "" : "/")) : string.Empty, bundle.Filename);
        }
    }
}
