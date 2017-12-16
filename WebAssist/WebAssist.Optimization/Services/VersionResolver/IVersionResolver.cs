namespace WebAssist.Optimizations
{
    public interface IVersionResolver
    {
        string GetVersionedPath(string virtualPath);
    }
}
