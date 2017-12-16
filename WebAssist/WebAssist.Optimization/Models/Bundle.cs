using System.Collections.Generic;

namespace WebAssist.Optimization
{
    public class Bundle
    {
        public string Name { get; set; }
        public string Filename { get; set; }
        public string SubPath { get; set; }
        public IReadOnlyList<string> Files { get; set; }
        public BundleTypeOption Type { get; set; }

        public string GetFullOutputPath(string outputDirectory)
        {
            var hasSubPath = !string.IsNullOrWhiteSpace(SubPath);
            return string.Concat(outputDirectory,
                hasSubPath ? (string.Concat(SubPath, SubPath.EndsWith("/") ? "" : "/")) : string.Empty,
                Filename);
        }
    }
}
