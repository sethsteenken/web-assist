using System.Collections.Generic;

namespace WebAssist.Optimizations
{
    public interface IBundleInterpreter
    {
        IEnumerable<ContentTag> DeterminePathsToRender(IEnumerable<string> pathsOrBundles);
    }
}
