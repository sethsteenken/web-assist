using System.Collections.Generic;

namespace WebAssist.Optimization
{
    public interface IBundleConfigurationBuilder
    {
        IReadOnlyList<BundleConfigInfo> GetConfigurations(string directory);
    }
}
