using Sirenix.OdinInspector;

namespace GCAssetFramework
{
    [System.Serializable]
    public class BundleModuleData
    {   
        /// <summary>
        /// AssetBundle 模块ID
        /// </summary> 
        public long bundleID;
        
        /// <summary>
        /// 模块名称
        /// </summary>
        public string moduleName;

        /// <summary>
        /// 是否打包
        /// </summary>
        public bool isBuild;

        /// <summary>
        /// 上一次双击按钮的时间
        /// </summary> 
        public float LastClickButtonime;

        /// <summary>
        /// 文件夹子包
        /// </summary>
        public string[] RootFolderPathArr;

        /// <summary>
        /// 预制体子包
        /// </summary>
        public string[] PrefabPathArr;

        /// <summary>
        /// 单个补丁包
        /// </summary>
        public BundleFileInfo[] SignFolderPathArr;
    }
    
    [System.Serializable]
    public class BundleFileInfo
    {   
        /// <summary>
        /// 包名称
        /// </summary>
        [HideLabel] public string abName = "AbName";

        /// <summary>
        /// 包路径
        /// </summary>
        [HideLabel][FolderPath] public string bundlePath = "BundlePath...";
    }
}
