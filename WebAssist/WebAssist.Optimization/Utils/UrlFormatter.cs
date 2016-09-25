using System.Web;

namespace WebAssist.Optimizations
{
    internal static class UrlFormatter
    {
        internal static string Url(string basePath, string path)
        {
            if (basePath != null)
                path = VirtualPathUtility.Combine(basePath, path);

            // Make sure it's not a ~/ path, which the client couldn't handle
            path = VirtualPathUtility.ToAbsolute(path);
            return HttpUtility.UrlPathEncode(path);
        }
    }
}
