using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GC.AssetFramework
{
    [CreateAssetMenu(fileName = "BundleSettings", menuName = "GC/AssetFramework/BundleSettings")]
    public class BundleSettings : ScriptableObject
    {
        #region 单例模式
        private static BundleSettings instance;
        
        public static BundleSettings Instance
        {
            get
            {
                if (instance == null) instance = Resources.Load<BundleSettings>("BundleSettings");
                return instance;
            }
        }
        #endregion

        [TitleGroup("资源加载热更设置"), LabelText("AssetBundle下载地址")]
        public string AssetBunleDownLoadURL;

        [TitleGroup("打包设置")]
        [LabelText("是否加密 AssetBundle")]
        public BundleEncryptToggle BundleEncrypt = new BundleEncryptToggle();

        [TitleGroup("打包设置")]
        [LabelText("打包目标平台")]
        public PlatformBuildTarget BuildTarget;

        [TitleGroup("打包设置")]
        [LabelText("打包压缩格式")]
        public BuildBundleOption BuildOptions;
    }


    [System.Serializable, Toggle("IsEncrypt")]
    public class BundleEncryptToggle
    {
        // 是否加密
        public bool IsEncrypt;

        // 加密密钥
        [LabelText("加密密钥")]
        public string EncryptKey;
    }

    public enum PlatformBuildTarget
    {
        Windows_64,
        
        MacOS,
        
        iOS,
        
        Android,
        
        Linux_64,
    }

    public static class Converter
    {
        public static BuildTarget TransPlatform(PlatformBuildTarget platform)
        {
            switch (platform)
            {
                case PlatformBuildTarget.Android:
                    return BuildTarget.Android;
                case PlatformBuildTarget.iOS:
                    return BuildTarget.iOS;
                case PlatformBuildTarget.Windows_64:
                    return BuildTarget.StandaloneWindows64;
                case PlatformBuildTarget.Linux_64:
                    return BuildTarget.StandaloneLinux64;
                case PlatformBuildTarget.MacOS:
                    return BuildTarget.StandaloneOSX;
                default:
                    return BuildTarget.Android;
            }
        }

        public static BuildAssetBundleOptions TransOption(BuildBundleOption buildBundleOption)
        {
            switch (buildBundleOption)
            {
                case BuildBundleOption.None:
                    return BuildAssetBundleOptions.None;
                case BuildBundleOption.UncompressedAssetBundle:
                    return BuildAssetBundleOptions.UncompressedAssetBundle;
                case BuildBundleOption.AppendHashToAssetBundleName:
                    return BuildAssetBundleOptions.AppendHashToAssetBundleName;
                case BuildBundleOption.DisableWriteTypeTree:
                    return BuildAssetBundleOptions.DisableWriteTypeTree;
                case BuildBundleOption.DryRunBuild:
                    return BuildAssetBundleOptions.DryRunBuild;
                case BuildBundleOption.IgnoreTypeTreeChanges:
                    return BuildAssetBundleOptions.IgnoreTypeTreeChanges;
                case BuildBundleOption.ChunkBasedCompression:
                    return BuildAssetBundleOptions.ChunkBasedCompression;
                case BuildBundleOption.StrictMode:
                    return BuildAssetBundleOptions.StrictMode;
                case BuildBundleOption.ForceRebuildAssetBundle:
                    return BuildAssetBundleOptions.ForceRebuildAssetBundle;
                case BuildBundleOption.DisableLoadAssetByFileName:
                    return BuildAssetBundleOptions.DisableLoadAssetByFileName;
                case BuildBundleOption.DisableLoadAssetByFileNameWithExtension:
                    return BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension;
                case BuildBundleOption.AssetBundleStripUnityVersion:
                    return BuildAssetBundleOptions.AssetBundleStripUnityVersion;
                case BuildBundleOption.UseContentHash:
                    return BuildAssetBundleOptions.UseContentHash;
                case BuildBundleOption.RecurseDependencies:
                    return BuildAssetBundleOptions.RecurseDependencies;
                case BuildBundleOption.StripUnatlasedSpriteCopies:
                    return BuildAssetBundleOptions.StripUnatlasedSpriteCopies;
                default:
                    return BuildAssetBundleOptions.ChunkBasedCompression;    
            }
        }
    }

    public enum BuildBundleOption
    {
        None,
        UncompressedAssetBundle,
        CollectDependencies,
        CompleteAssets,
        DisableWriteTypeTree,
        DeterministicAssetBundle,
        ForceRebuildAssetBundle,
        IgnoreTypeTreeChanges,
        AppendHashToAssetBundleName,
        ChunkBasedCompression,
        StrictMode,
        DryRunBuild,
        DisableLoadAssetByFileName,
        DisableLoadAssetByFileNameWithExtension,
        AssetBundleStripUnityVersion,
        UseContentHash,
        RecurseDependencies,
        StripUnatlasedSpriteCopies
    }
}
