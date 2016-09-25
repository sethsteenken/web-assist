using System.Linq;

namespace WebAssist.Optimizations
{
    public class BundlesManager
    {
        internal void Add(BundleConfigFile config)
        {
            if (!BundleSettings.BundleConfigurations.Where(x => x.Path == config.Path).Any())
            {
                config.UpdateContentFromConfiguration();
                BundleSettings.BundleConfigurations.Add(config);
            }
        }
    }
}
