using System;
using System.Web;

namespace WebAssist.Optimization
{
    public sealed class ContentHelper
    {
        public const string DefaultScriptTagFormat = @"<script src=""{0}"" type=""text/javascript""></script>";
        public const string DefaultStyleTagFormat = @"<link href=""{0}"" rel=""stylesheet""/>";

        private readonly IContentManager manager;

        public ContentHelper(IContentManager manager)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));

            this.manager = manager;
        }

        public IHtmlString RenderScripts(params string[] pathsOrBundles)
        {
            return RenderFormat(DefaultScriptTagFormat, pathsOrBundles);
        }

        public IHtmlString RenderStyles(params string[] pathsOrBundles)
        {
            return RenderFormat(DefaultStyleTagFormat, pathsOrBundles);
        }

        public IHtmlString RenderFormat(string tagFormat, params string[] pathsOrBundles)
        {
            return manager.Render(tagFormat, pathsOrBundles);
        }

        public static ContentHelper Create() => Create(HttpContext.Current);
        public static ContentHelper Create(HttpContext httpContext) => Create(new HttpContextWrapper(httpContext));
        public static ContentHelper Create(HttpContextBase httpContext)
        {
            var manager = ContentManagerFactory.GetInstance(httpContext);
            return new ContentHelper(manager);
        }
    }
}
