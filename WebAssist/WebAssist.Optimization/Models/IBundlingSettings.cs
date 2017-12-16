namespace WebAssist.Optimization
{
    public interface IBundlingSettings
    {
        bool Enabled { get; }
        string DefinitionsDirectory { get; }
        string JSOutputDirectory { get; }
        string CSSOutputDirectory { get; }
        bool UseVersioning { get; }
    }
}
