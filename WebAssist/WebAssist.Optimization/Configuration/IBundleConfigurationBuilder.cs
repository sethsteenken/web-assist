using System.Collections.Generic;

namespace WebAssist.Optimizations
{
    public interface IBundleConfigurationBuilder
    {
        IReadOnlyList<BundleConfigInfo> GetConfigurations(string directory);
    }
}
