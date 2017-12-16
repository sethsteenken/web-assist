using System;

namespace WebAssist.Optimizations
{
    public sealed class BundlingSettings : IBundlingSettings
    {
        internal static IBundlingSettings CustomSettings;

        public bool Enabled { get; set; }
        public string JSOutputDirectory { get; set; }
        public string CSSOutputDirectory { get; set; }
        public bool UseVersioning { get; set; }
        public string ConfigurationsDirectory { get; set; }

        public static void ApplySettings(IBundlingSettings settings)
        {
            CustomSettings = settings ?? throw new ArgumentNullException(nameof(IBundlingSettings));
        }
    }
}
