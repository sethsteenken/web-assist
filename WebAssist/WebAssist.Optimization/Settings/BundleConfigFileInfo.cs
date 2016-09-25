using System.Collections.Generic;

namespace WebAssist.Optimizations
{
    public class BundleConfigFileInfo
    {
        public IList<Bundle> Scripts { get; set; }
        public IList<Bundle> Styles { get; set; }
    }
}
