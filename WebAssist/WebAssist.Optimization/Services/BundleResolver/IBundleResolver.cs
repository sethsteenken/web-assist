using System.Collections.Generic;

namespace WebAssist.Optimizations
{
    public interface IBundleResolver
    {
        IEnumerable<string> GetBundleContents(string name);
        string GetBundleUrl(Bundle bundle);
        bool TryGetBundle(string name, out Bundle bundle);
    }
}
