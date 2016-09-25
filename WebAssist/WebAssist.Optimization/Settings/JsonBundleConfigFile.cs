using Newtonsoft.Json;
using System.IO;

namespace WebAssist.Optimizations
{
    internal class JsonBundleConfigFile : BundleConfigFile
    {
        public JsonBundleConfigFile(string path) : base(path)
        {

        }
        public override void UpdateContentFromConfiguration()
        {
            using (StreamReader file = File.OpenText(Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                Content = (BundleConfigFileInfo)serializer.Deserialize(file, typeof(BundleConfigFileInfo));
            }

            CleanBundleInfo();
        }
    }
}
