using System.Collections.Generic;

namespace WebAssist.Optimization
{
    public interface IBundleResolver
    {
        IEnumerable<string> GetBundleContents(string name);
        string GetBundleUrl(Bundle bundle);
        bool TryGetBundle(string name, out Bundle bundle);
    }
}
