using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace WebAssist.Optimizations
{
    public class BundleConfigurationBuilder : IBundleConfigurationBuilder
    {
        private readonly JsonSerializer _serializer;
        private static IReadOnlyList<BundleConfigInfo> _cachedConfigurations;

        public BundleConfigurationBuilder(JsonSerializer serializer)
        {
            _serializer = serializer;
        }

        public IReadOnlyList<BundleConfigInfo> GetConfigurations(string directory)
        {
            if (_cachedConfigurations != null)
                return _cachedConfigurations;

            var bundleConfigs = new List<BundleConfigInfo>();
            var directoryInfo = new DirectoryInfo(directory);
            if (directoryInfo.Exists)
            {
                var configFiles = directoryInfo.GetFiles("*.json");
                foreach (var file in configFiles)
                {
                    bundleConfigs.Add(GetConfigInfoFromFile(file.FullName));
                }
            }

            if (bundleConfigs.Count > 0)
                _cachedConfigurations = new List<BundleConfigInfo>(bundleConfigs);

            return _cachedConfigurations ?? new List<BundleConfigInfo>();
        }

        private BundleConfigInfo GetConfigInfoFromFile(string path)
        {
            using (StreamReader file = File.OpenText(path))
            {
                var content = (BundleConfigInfo)_serializer.Deserialize(file, typeof(BundleConfigInfo));
                content.Clean();
                return content;
            }
        }
    }
}
