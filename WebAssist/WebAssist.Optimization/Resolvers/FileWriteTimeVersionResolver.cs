using System.IO;
using System.Web;

namespace WebAssist.Optimizations
{
    public class FileWriteTimeVersionResolver : IVersionResolver
    {
        public string GetVersionedPath(string virtualPath)
        {
            var path = HttpContext.Current.Server.MapPath(virtualPath);
            var date = File.GetLastWriteTime(path);

            return string.Format("{0}?v={1:yyyyMMddhhmmss}", virtualPath, date);
        }
    }
}
