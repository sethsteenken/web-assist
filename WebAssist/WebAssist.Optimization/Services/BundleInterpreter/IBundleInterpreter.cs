using System.Collections.Generic;

namespace WebAssist.Optimization
{
    public interface IBundleInterpreter
    {
        IEnumerable<ContentTag> DeterminePathsToRender(IEnumerable<string> pathsOrBundles);
    }
}
