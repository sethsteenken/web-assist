namespace WebAssist.Optimizations
{
    public interface IBundlingSettings
    {
        bool Enabled { get; }
        string ConfigurationsDirectory { get; }
        string JSOutputDirectory { get; }
        string CSSOutputDirectory { get; }
        bool UseVersioning { get; }
    }
}
