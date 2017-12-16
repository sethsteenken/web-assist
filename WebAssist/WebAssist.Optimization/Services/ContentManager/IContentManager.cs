using System.Web;

namespace WebAssist.Optimization
{
    public interface IContentManager
    {
        IHtmlString Render(string tagFormat, params string[] pathsOrBundles);
    }
}
