using System;
using System.Web;

namespace WebAssist.Optimizations
{
    public class UrlPathFormatter : IPathFormatter
    {
        private readonly HttpRequestBase _request;

        public UrlPathFormatter(HttpRequestBase request)
        {
            _request = request;
        }

        public string ResolveVirtualPath(string virtualPath)
        {
            Uri uri;
            if (Uri.TryCreate(virtualPath, UriKind.Absolute, out uri))
                return virtualPath;

            string basePath = "";
            if (_request != null)
                basePath = _request.AppRelativeCurrentExecutionFilePath;

            return FormatUrl(basePath, virtualPath);
        }

        private static string FormatUrl(string basePath, string path)
        {
            if (basePath != null)
                path = VirtualPathUtility.Combine(basePath, path);

            // Make sure it's not a ~/ path, which the client couldn't handle
            path = VirtualPathUtility.ToAbsolute(path);
            return HttpUtility.UrlPathEncode(path);
        }
    }
}
