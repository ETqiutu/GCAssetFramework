using UnityEngine;

namespace GC.AssetFramework
{
    public enum BuildType
    {
        AssetBundle,
        HotPatch
    }

    public class BuildBundleCompiler
    {
        /// <summary>
        /// 打包AssetBundle
        /// </summary>
        /// <param name="moduleData">资源模块的配置数据</param>
        /// <param name="buildType">构建类型</param>
        /// <param name="hotPathVersion">热更新版本</param>
        /// <param name="updateNotice">热更新公告</param> 
        public static void BuildAssetBundle(BundleModuleData moduleData,BuildType buildType = BuildType.AssetBundle, int hotPathVersion = 0, string updateNotice = "")
        {
            
        }   
    }
}
