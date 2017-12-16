using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAssist.Optimizations
{
    public class BundleResolver : IBundleResolver
    {
        private readonly IVersionResolver _versioningResolver;
        private readonly IBundlingSettings _bundlingSettings;
        private readonly IReadOnlyList<BundleConfigInfo> _configurations;

        public BundleResolver(IBundlingSettings bundlingSettings, 
            IVersionResolver versioningResolver, 
            IReadOnlyList<BundleConfigInfo> configurations)
        {
            _versioningResolver = versioningResolver;
            _bundlingSettings = bundlingSettings;
            _configurations = configurations ?? new List<BundleConfigInfo>();
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

            if (_bundlingSettings.UseVersioning)
                virtualPath = _versioningResolver.GetVersionedPath(virtualPath);

            return virtualPath;
        }

        public bool TryGetBundle(string name, out Bundle bundle)
        {
            bundle = FindBundle(name);
            return bundle != null;
        }

        private Bundle FindBundle(string pathOrName)
        {
            foreach (var config in _configurations)
            {
                var bundle = config.Scripts.Where(x => string.Compare(x.Name, pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                                       string.Compare(x.Filename, pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                                       string.Compare(string.Concat(_bundlingSettings.JSOutputDirectory, x.SubPath, x.Filename), pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0)
                                            .FirstOrDefault();

                if (bundle == null)
                {
                    bundle = config.Styles.Where(x => string.Compare(x.Name, pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                                      string.Compare(x.Filename, pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                                                      string.Compare(string.Concat(_bundlingSettings.CSSOutputDirectory, x.SubPath, x.Filename), pathOrName, StringComparison.InvariantCultureIgnoreCase) == 0)
                                            .FirstOrDefault();

                    if (bundle != null)
                        bundle.Type = BundleTypeOption.CSS;
                }
                else
                    bundle.Type = BundleTypeOption.JS;

                if (bundle != null)
                    return bundle;
            }

            return null;
        }

        private string GetBundleFullPath(Bundle bundle)
        {   
            switch (bundle.Type)
            {
                case BundleTypeOption.JS:
                    return bundle.GetFullOutputPath(_bundlingSettings.JSOutputDirectory);
                case BundleTypeOption.CSS:
                    return bundle.GetFullOutputPath(_bundlingSettings.CSSOutputDirectory);
                default:
                    return string.Empty;
            }
        }
    }
}
