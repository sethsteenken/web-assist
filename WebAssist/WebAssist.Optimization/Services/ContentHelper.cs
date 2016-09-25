using System.Web;
using System;

namespace WebAssist.Optimizations
{
    public class ContentHelper
    {
        private const string _defaultScriptTagFormat = @"<script src=""{0}"" type=""text/javascript""></script>";
        private const string _defaultStyleTagFormat = @"<link href=""{0}"" rel=""stylesheet""/>";

        internal ContentHelper(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            Manager = ContentManager.GetInstance(httpContext);
        }

        internal ContentManager Manager { get; private set; }

        public static ContentHelper Create() => Create(HttpContext.Current);
        public static ContentHelper Create(HttpContext httpContext) => Create(new HttpContextWrapper(httpContext));
        public static ContentHelper Create(HttpContextBase httpContext) => new ContentHelper(httpContext);

        public IHtmlString RenderScripts(params string[] pathsOrBundles)
        {
            return RenderFormat(_defaultScriptTagFormat, pathsOrBundles);
        }

        public IHtmlString RenderStyles(params string[] pathsOrBundles)
        {
            return RenderFormat(_defaultStyleTagFormat, pathsOrBundles);
        }

        public IHtmlString RenderFormat(string tagFormat, params string[] pathsOrBundles)
        {
            return Manager.Render(tagFormat, pathsOrBundles);
        }
    }
}
