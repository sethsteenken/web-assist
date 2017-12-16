using System.IO;
using System.Web;

namespace WebAssist.Optimization
{
    public class FileWriteTimeVersionResolver : IVersionResolver
    {
        private readonly HttpServerUtilityBase server;

        public FileWriteTimeVersionResolver(HttpServerUtilityBase server)
        {
            this.server = server;
        }

        public string GetVersionedPath(string virtualPath)
        {
            var date = File.GetLastWriteTime(server.MapPath(virtualPath));
            return string.Format("{0}?v={1:yyyyMMddhhmmss}", virtualPath, date);
        }
    }
}
