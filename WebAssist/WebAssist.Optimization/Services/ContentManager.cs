using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebAssist.Optimizations
{
    internal sealed class ContentManager
    {
        internal static readonly object ContentManagerKey = typeof(ContentManager);
        private readonly IBundleResolver _resolver;

        public ContentManager(HttpContextBase context, IBundleResolver resolver)
        {
            _httpContext = context;
            _resolver = resolver;
        }

        private readonly HttpContextBase _httpContext;

        public static ContentManager GetInstance(HttpContextBase context)
        {
            if (context == null)
                return null;

            var manager = (ContentManager)context.Items[ContentManagerKey];
            if (manager == null)
            {
                manager = new ContentManager(context, new BundleResolver(BundleSettings.VersioningResolver));
                context.Items[ContentManagerKey] = manager;
            }
            return manager;
        }

        public IHtmlString Render(string tagFormat, params string[] pathsOrBundles)
        {
            ValidateParameters(tagFormat, pathsOrBundles);

            IEnumerable<ContentTag> uniqueRefs = DeterminePathsToRender(pathsOrBundles);

            StringBuilder result = new StringBuilder();
            foreach (ContentTag r in uniqueRefs)
            {
                result.Append(r.Render(tagFormat));
                result.Append(Environment.NewLine);
            }

            return new HtmlString(result.ToString());
        }

        private void ValidateParameters(string tagFormat, params string[] pathsOrBundles)
        {
            if (string.IsNullOrEmpty(tagFormat))
                throw new ArgumentNullException(nameof(tagFormat));
            if (pathsOrBundles == null)
                throw new ArgumentNullException(nameof(pathsOrBundles));

            foreach (string path in pathsOrBundles)
            {
                if (string.IsNullOrEmpty(path))
                    throw new ArgumentNullException(nameof(path));
            }
        }

        private IEnumerable<ContentTag> DeterminePathsToRender(IEnumerable<string> assets)
        {
            var paths = new List<ContentTag>();
            BuildPaths(ref paths, assets);
            return EliminateDuplicatesAndResolveUrls(paths);
        }

        private void BuildPaths(ref List<ContentTag> paths, IEnumerable<string> assets)
        {
            foreach (string asset in assets)
            {
                Bundle bundle;
                if (_resolver.TryGetBundle(asset, out bundle))
                {
                    if (BundleSettings.Enabled)
                    {
                        var tag = new ContentTag(_resolver.GetBundleUrl(bundle));
                        tag.IsBundle = true;
                        paths.Add(tag);
                    }
                    else
                    {
                        BuildPaths(ref paths, bundle.Files);
                    }
                }
                else
                {
                    paths.Add(new ContentTag(asset));
                }
            }
        }

        private IEnumerable<ContentTag> EliminateDuplicatesAndResolveUrls(IEnumerable<ContentTag> refs)
        {
            List<ContentTag> firstPass = new List<ContentTag>();
            HashSet<string> pathMap = new HashSet<string>();
            HashSet<string> bundledContents = new HashSet<string>();

            // first eliminate any duplicate paths
            foreach (ContentTag asset in refs)
            {
                // Leave static assets alone
                if (asset.IsStatic)
                {
                    firstPass.Add(asset);
                    continue;
                }

                string path = asset.Value;
                if (!pathMap.Contains(path))
                {
                    // Need to crack open bundles to look at its contents for the second pass
                    if (asset.IsBundle)
                    {
                        IEnumerable<string> contents = _resolver.GetBundleContents(path);
                        foreach (string filePath in contents)
                        {
                            bundledContents.Add(ResolveVirtualPath(filePath));
                        }
                    }

                    string resolvedPath = ResolveVirtualPath(path);
                    if (!pathMap.Contains(resolvedPath))
                    {
                        pathMap.Add(resolvedPath);
                        asset.Value = resolvedPath;
                        firstPass.Add(asset);
                    }

                    pathMap.Add(path);
                }
            }

            // Second pass to eliminate files that are contained inside of bundles
            return firstPass.Where(asset => asset.IsStatic || !bundledContents.Contains(asset.Value));
        }

        private string ResolveVirtualPath(string virtualPath)
        {
            Uri uri;
            if (Uri.TryCreate(virtualPath, UriKind.Absolute, out uri))
                return virtualPath;

            string basePath = "";
            if (_httpContext.Request != null)
                basePath = _httpContext.Request.AppRelativeCurrentExecutionFilePath;

            return UrlFormatter.Url(basePath, virtualPath);
        }
    }
}
