using System.Collections.Generic;

namespace WebAssist.Optimization
{
    public class BundleConfigInfo
    {
        protected BundleConfigInfo() { }
        public BundleConfigInfo(IReadOnlyList<Bundle> scripts, IReadOnlyList<Bundle> styles)
        {
            Scripts = scripts;
            Styles = styles;

            Clean();
        }

        public IReadOnlyList<Bundle> Scripts { get; private set; }
        public IReadOnlyList<Bundle> Styles { get; private set; }

        public void Clean()
        {
            if (Scripts != null)
            {
                foreach (var bundle in Scripts)
                {
                    if (string.IsNullOrWhiteSpace(bundle.Filename))
                        bundle.Filename = string.Concat(bundle.Name, ".min.js");
                }
            }

            if (Styles != null)
            {
                foreach (var bundle in Styles)
                {
                    if (string.IsNullOrWhiteSpace(bundle.Filename))
                        bundle.Filename = string.Concat(bundle.Name, ".min.css");
                }
            }
        }
    }
}
