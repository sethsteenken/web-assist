using System.Collections.Generic;

namespace WebAssist.Optimizations
{
    public class Bundle
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public string SubPath { get; set; }
        public IReadOnlyList<string> Files { get; set; }
        public BundleType Type { get; set; }
    }
}
