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
        AppendHashToAssetBundleName,
        AssetBundleStripUnityVersion,
        ChunkBasedCompression,
        DisableLoadAssetByFileName,
        DisableLoadAssetByFileNameWithExtension,
        DisableWriteTypeTree,
        DryRunBuild,
        ForceRebuildAssetBundle,
        IgnoreTypeTreeChanges,
        None,
        RecurseDependencies,
        StrictMode,
        StripUnatlasedSpriteCopies,
        UncompressedAssetBundle,
        UseContentHash
    }

    public enum DownloadStatus
    {
        Pending,
        Downloading,
        Completed,
        Failed,
        Paused
    }
    #if UNITY_EDITOR
    public class Utilities
    {
        public static UnityEditor.BuildTarget GetBuildTarget(BuildPlatform buildPlatform)
        {
            switch (buildPlatform)
            {
                case BuildPlatform.IOS:
                    return UnityEditor.BuildTarget.iOS;
                case BuildPlatform.MacOS:
                    return UnityEditor.BuildTarget.StandaloneOSX;
                case BuildPlatform.Windows:
                    return UnityEditor.BuildTarget.StandaloneWindows64;
                case BuildPlatform.Linux:
                    return UnityEditor.BuildTarget.StandaloneLinux64;
                case BuildPlatform.Android:
                    return UnityEditor.BuildTarget.Android;
                default:
                    return UnityEditor.BuildTarget.Android;
            }
        }

        public static UnityEditor.BuildAssetBundleOptions GetBundleOptions(BuildOption buildOption)
        {
            switch (buildOption)
            {
                case BuildOption.AppendHashToAssetBundleName:
                    return UnityEditor.BuildAssetBundleOptions.AppendHashToAssetBundleName;
                case BuildOption.AssetBundleStripUnityVersion:
                    return UnityEditor.BuildAssetBundleOptions.AssetBundleStripUnityVersion;
                case BuildOption.ChunkBasedCompression:
                    return UnityEditor.BuildAssetBundleOptions.ChunkBasedCompression;
                case BuildOption.DisableLoadAssetByFileName:
                    return UnityEditor.BuildAssetBundleOptions.DisableLoadAssetByFileName;
                case BuildOption.DisableLoadAssetByFileNameWithExtension:
                    return UnityEditor.BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension;
                case BuildOption.DisableWriteTypeTree:
                    return UnityEditor.BuildAssetBundleOptions.DisableWriteTypeTree;
                case BuildOption.DryRunBuild:
                    return UnityEditor.BuildAssetBundleOptions.DryRunBuild;
                case BuildOption.ForceRebuildAssetBundle:
                    return UnityEditor.BuildAssetBundleOptions.ForceRebuildAssetBundle;
                case BuildOption.IgnoreTypeTreeChanges:
                    return UnityEditor.BuildAssetBundleOptions.IgnoreTypeTreeChanges;
                case BuildOption.None:
                    return UnityEditor.BuildAssetBundleOptions.None;
                case BuildOption.RecurseDependencies:
                    return UnityEditor.BuildAssetBundleOptions.RecurseDependencies;
                case BuildOption.StrictMode:
                    return UnityEditor.BuildAssetBundleOptions.StrictMode;
                case BuildOption.StripUnatlasedSpriteCopies:
                    return UnityEditor.BuildAssetBundleOptions.StripUnatlasedSpriteCopies;
                case BuildOption.UncompressedAssetBundle:
                    return UnityEditor.BuildAssetBundleOptions.UncompressedAssetBundle;
                case BuildOption.UseContentHash:
                    return UnityEditor.BuildAssetBundleOptions.UseContentHash;
                default:
                    return UnityEditor.BuildAssetBundleOptions.ChunkBasedCompression;             
            }
        }
    }
    #endif
}
