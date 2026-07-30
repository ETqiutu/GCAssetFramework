namespace GC.AssetsFramework
{
    public enum BuildPlatform
    {
        IOS,
        MacOS,
        Windows,
        Linux,
        Android
    }

    public enum BuildOption
    {
        
    }

    public enum DownloadStatus
    {
        Pending,
        Downloading,
        Completed,
        Failed,
        Paused
    }
}
