using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace GC.AssetFramework
{
    public enum BuildType
    {
        HotPatch,
        AssetBundle
    }

    public class BuildBundleCompiler 
    {
        /// <summary>
        /// 热更更新版本
        /// </summary> 
        private static int Version;

        /// <summary>
        /// 热更更细公告
        /// </summary>
        private static string UpdateNotice;
        
        /// <summary>
        /// 构建类型，是否为热更
        /// </summary>
        private static BuildType BuildType;
        
        /// <summary>
        /// 构建数据：构建的原始数据
        /// </summary>
        private static ModuleData TargetData;

        /// <summary>
        /// 所有资源路径（包含预制体）
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private static List<string> AllFilePath = new List<string>();

        /// <summary>
        /// 所有的资源路径（不包含预制体）
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, List<string>> AllAssets = new Dictionary<string, List<string>>();

        /// <summary>
        /// 所有的预制体的路径
        /// </summary> 
        /// <returns></returns>
        private static Dictionary<string, List<string>> AllPrefabs = new Dictionary<string, List<string>>();

        /// <summary>
        /// 执行打包逻辑
        /// </summary>
        /// <param name="moduleData">模块数据</param>
        /// <param name="buildType">构建类型</param>
        /// <param name="version">热更版本</param>
        /// <param name="notice">更新公告</param>
        public static void BuildAssetBundle(ModuleData moduleData, BuildType buildType = BuildType.AssetBundle, int version = 0, string notice = "")
        {
            Initialize(moduleData, buildType, version, notice);
            BuildAssets();
            BuildPrefab();
            BuildAllAssetBundle();
        }

        /// <summary>
        /// 初始化打包任务
        /// </summary>
        /// <param name="moduleData">模块数据</param>
        /// <param name="buildType">构建类型</param>
        /// <param name="version">热更版本</param>
        /// <param name="notice">更新公告</param>
        private static void Initialize(ModuleData moduleData, BuildType buildType = BuildType.AssetBundle, int version = 0, string notice = "")
        {
            AllFilePath.Clear();

            TargetData = moduleData;
            BuildType = buildType;
            Version = version;
            UpdateNotice = notice;
        }

        /// <summary>
        /// 构建游戏资源
        /// </summary>
        private static void BuildAssets()
        {
            if (TargetData.Packages == null || TargetData.Packages.Length == 0)
                return;

            foreach (var package in TargetData.Packages)
            {
                if (package.AssetsPath == null || package.AssetsPath.Length == 0)
                    continue;
                
                foreach (var path in package.AssetsPath)
                {
                    string filePath = path.Replace(@"\", "/");
                    if (!IsRequired(filePath))
                    {
                        AllFilePath.Add(filePath);
                        string package_name = GeneratePackageName(package.PackageName);
                        if (!AllAssets.ContainsKey(package_name))
                        {
                            AllAssets.Add(package_name, new List<string>{ filePath });
                        }
                        else
                        {
                            AllAssets[package_name].Add(filePath);
                        }
                    }
                    else
                    {
                        Debug.LogError($"GC Asset Framework: There are redundant items: {filePath}");
                    }
                }
            }
        }
        
        /// <summary>
        /// 构建预制体
        /// </summary>
        private static void BuildPrefab()
        {
            if (TargetData.Packages == null || TargetData.Packages.Length == 0)
                return;
            
            foreach (var package in TargetData.Packages)
            {
                string[] guidArr = AssetDatabase.FindAssets("t:Prefab", package.Prefabs);
                for (int i = 0; i < guidArr.Length; i ++)
                {
                    string filePath = AssetDatabase.GUIDToAssetPath(guidArr[i]);
                    string package_name = GeneratePackageName(package.PackageName);
                    if (!AllFilePath.Contains(filePath))
                    {
                        AllFilePath.Add(filePath);
                        string[] dependsArr = AssetDatabase.GetDependencies(filePath);
                        List<string> dependsList = new List<string>();
                        for (int j = 0; j < dependsArr.Length; j++)
                        {
                            string dependpath = dependsArr[j];
                            if (!IsRequired(dependpath))
                            {
                                AllFilePath.Add(dependpath);
                                dependsList.Add(dependpath);
                                if (dependpath.EndsWith(".prefab"))
                                {
                                    if (!AllPrefabs.ContainsKey(package_name))
                                    {
                                        AllPrefabs.Add(package_name, new List<string>{ dependpath });
                                    }
                                    else
                                    {
                                        AllPrefabs[package_name].Add(dependpath);
                                    }
                                }
                                else
                                {
                                    if (!AllAssets.ContainsKey(package_name))
                                    {
                                        AllAssets.Add(package_name, new List<string>{ dependpath });
                                    }
                                    else
                                    {
                                        AllAssets[package_name].Add(dependpath);
                                    }
                                }
                            }
                            else
                            {
                                Debug.LogError($"GC Asset Framework: There are redundant items in the packaged dependencies: {filePath}");
                            }
                        }
                        if (!AllPrefabs.ContainsKey(package_name))
                        {
                            AllPrefabs.Add(package_name, new List<string>{ filePath });
                        }
                        else
                        {
                            AllPrefabs[package_name].Add(filePath);
                        }
                    }
                    else
                    {
                        Debug.LogError($"GC Asset Framework: There are redundant items: {filePath}");
                    }
                }
            }
        }

        /// <summary>
        /// 构建AssetBundle资源包
        /// </summary>
        private static void BuildAllAssetBundle()
        {
            ModifyAllFileBundleName();
        }

        /// <summary>
        /// 进行打包名称
        /// </summary>
        private static void ModifyAllFileBundleName()
        {
            int part = 0;
            foreach (var assets in AllAssets)
            {
                foreach (var asset in assets.Value)
                {
                    part ++;
                    AssetImporter importer = AssetImporter.GetAtPath(asset);
                    if (importer != null)
                    {
                        importer.assetBundleName = assets.Key;
                    }
                }
            }
            part = 0;
            foreach (var prefabs in AllPrefabs)
            {
                foreach (var prefab in prefabs.Value)
                {
                    part ++;
                    AssetImporter importer = AssetImporter.GetAtPath(prefab);
                    if (importer != null)
                    {
                        importer.assetBundleName = prefabs.Key;
                    }
                }
            }
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }

        /// <summary>
        /// 确定路径是否为冗余文件
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private static bool IsRequired(string Path)
        {
            foreach (var filePath in AllFilePath)
            {
                if (string.Equals(filePath, Path) || filePath.Contains(Path) || Path.EndsWith(".cs"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 生成包名称
        /// </summary>
        /// <param name="PackageName"></param>
        /// <returns></returns>
        private static string GeneratePackageName(string PackageName)
        {
            return TargetData.ModuleName + "_" + PackageName;
        }
    }
}
