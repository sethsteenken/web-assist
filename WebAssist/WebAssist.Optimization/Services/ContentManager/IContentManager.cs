using System.Web;

namespace WebAssist.Optimizations
{
    public interface IContentManager
    {
        IHtmlString Render(string tagFormat, params string[] pathsOrBundles);
    }
}
