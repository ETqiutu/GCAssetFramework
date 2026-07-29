using System;
using UnityEngine;

namespace GC.AssetFramework
{
    /// <summary>
    /// 热更资源模块
    /// </summary>
    public class HotAssetsModule
    {
        /// <summary>
        /// 下载资源模块
        /// </summary> 
        public string CurrentBundleModule { get; set; }

        public HotAssetsModule(string BundleModule)
        {
            CurrentBundleModule = BundleModule;
        }

        /// <summary>
        /// 开始热更资源
        /// </summary>
        /// <param name="startDownLoadCallback">开始下载回调</param>
        /// <param name="hotFinish">热更完成回调</param>
        /// <param name="ischeckAssetsVersion">是否检测资源版本</param> 
        public void StartHotAssets(Action startDownLoadCallback, Action<string> hotFinish = null, bool ischeckAssetsVersion = true)
        {
            
        }
    }
}
