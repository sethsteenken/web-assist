using System.Collections.Generic;
using System.Linq;

namespace WebAssist.Optimization
{
    public class BundleInterpreter : IBundleInterpreter
    {
        private readonly IBundleResolver _resolver;
        private readonly IPathFormatter _pathFormatter;
        private readonly IBundlingSettings _bundlingSettings;

        public BundleInterpreter(IBundleResolver resolver, IPathFormatter pathFormatter, IBundlingSettings bundlingSettings)
        {
            _pathFormatter = pathFormatter;
            _resolver = resolver;
            _bundlingSettings = bundlingSettings;
        }

        public virtual IEnumerable<ContentTag> DeterminePathsToRender(IEnumerable<string> pathsOrBundles)
        {
            var paths = new List<ContentTag>();
            BuildPaths(paths, pathsOrBundles);
            return EliminateDuplicatesAndResolveUrls(paths);
        }

        protected virtual void BuildPaths(List<ContentTag> paths, IEnumerable<string> references)
        {
            foreach (string contentReference in references)
            {
                Bundle bundle;
                if (_resolver.TryGetBundle(contentReference, out bundle))
                {
                    if (_bundlingSettings.Enabled)
                        paths.Add(new ContentTag(_resolver.GetBundleUrl(bundle), isBundle: true));
                    else
                        BuildPaths(paths, bundle.Files);
                }
                else
                {
                    paths.Add(new ContentTag(contentReference));
                }
            }
        }

        protected virtual IEnumerable<ContentTag> EliminateDuplicatesAndResolveUrls(IEnumerable<ContentTag> tags)
        {
            var firstPass = new List<ContentTag>();
            var pathMap = new HashSet<string>();
            var bundledContents = new HashSet<string>();

            // first eliminate any duplicate paths
            foreach (ContentTag tag in tags)
            {
                // Leave static assets alone
                if (tag.IsStatic)
                {
                    firstPass.Add(tag);
                    continue;
                }

                string path = tag.Value;
                if (!pathMap.Contains(path))
                {
                    // Need to crack open bundles to look at its contents for the second pass
                    if (tag.IsBundle)
                    {
                        // add the bundled contents to list to check so those scripts are not duplicated in the resulting list
                        IEnumerable<string> contents = _resolver.GetBundleContents(path);
                        foreach (string filePath in contents)
                        {
                            bundledContents.Add(_pathFormatter.ResolveVirtualPath(filePath));
                        }
                    }

                    string resolvedPath = _pathFormatter.ResolveVirtualPath(path);
                    if (!pathMap.Contains(resolvedPath))
                    {
                        pathMap.Add(resolvedPath);
                        tag.UpdateValue(resolvedPath);
                        firstPass.Add(tag);
                    }

                    pathMap.Add(path);
                }
            }

            // Second pass to eliminate files that are contained inside of bundles
            return firstPass.Where(asset => asset.IsStatic || !bundledContents.Contains(asset.Value));
        }
    }
}
