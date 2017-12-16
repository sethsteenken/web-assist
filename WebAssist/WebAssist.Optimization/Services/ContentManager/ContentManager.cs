using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace WebAssist.Optimizations
{
    public sealed class ContentManager : IContentManager
    {
        internal static readonly object ContextKey = typeof(ContentManager);
        private readonly IBundleInterpreter _bundleInterpreter;

        public ContentManager(IBundleInterpreter bundleInterpreter)
        {
            _bundleInterpreter = bundleInterpreter;
        }

        public IHtmlString Render(string tagFormat, params string[] pathsOrBundles)
        {
            ValidateParameters(tagFormat, pathsOrBundles);

            IEnumerable<ContentTag> uniqueRefs = _bundleInterpreter.DeterminePathsToRender(pathsOrBundles);

            var result = new StringBuilder();
            foreach (ContentTag tag in uniqueRefs)
            {
                result.Append(tag.Render(tagFormat));
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
    }
}
