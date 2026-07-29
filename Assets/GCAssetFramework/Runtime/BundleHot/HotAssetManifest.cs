using System.Collections.Generic;
using UnityEngine;

namespace GC.AssetFramework
{
    /// <summary>
    /// 热更资源清单
    /// </summary>
    public class HotAssetManifest
    {
        /// <summary>
        /// 更新公告
        /// </summary>
        public string UpdateNotice;

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadURL;

        /// <summary>
        /// 热更资源补丁列表
        /// </summary>
        /// <typeparam name="HotAssetPatch"></typeparam>
        /// <returns></returns>
        public List<HotAssetPatch> HotAssetsPatchList = new List<HotAssetPatch>();
    }

    /// <summary>
    /// 热更文件信息
    /// </summary>
    public class HotFileInfo
    {
        public string ABName; // AB包名称
        public string MD5;    // 文件的MD5
        public float Size;    // 文件大小
    }
    
    /// <summary>
    /// 热更补丁
    /// </summary>
    public class HotAssetPatch
    {
        /// <summary>
        /// 补丁版本
        /// </summary>
        public int PatchVersion;

        /// <summary>
        /// 热更资源列表
        /// </summary>
        public List<HotFileInfo> HotAssetList = new List<HotFileInfo>();
    }
}
