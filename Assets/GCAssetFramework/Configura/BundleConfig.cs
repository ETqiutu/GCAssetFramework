using System.Collections.Generic;
using UnityEngine;

namespace GC.AssetFramework
{
    /// <summary>
    /// 资源配置路径
    /// </summary>
    [System.Serializable]
    public class BundleConfig
    {
        /// <summary>
        /// 所有AssetBundle的信息列表
        /// </summary>
        public List<BundleInfo> BundleInfoList;
    }
    
    /// <summary>
    /// AssetBundle 名称
    /// </summary> 
    [System.Serializable]
    public class BundleInfo
    {
        
        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path;
        
        /// <summary>
        /// Crc
        /// </summary>
        public uint crc;
        
        /// <summary>
        /// AssetBundle名称
        /// </summary>
        public string BundleName;
        
        /// <summary>
        /// 资源名称
        /// </summary>
        public string AssetName;

        /// <summary>
        /// 依赖项
        /// </summary>
        public List<string> BundleDependce;
    }

    /// <summary>
    /// 内嵌的AssetBundle的信息
    /// </summary> 
    [System.Serializable]
    public class BuildInSideBundleInfo
    {
        public string FileName; // 文件名称
        public string MD5;      // 校验本地文件与包内文件一致
        public float Size;      // 文件大小
    }
}
