using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
        /// AssetBundle 打包文件输出路径
        /// </summary>
        public static string BundleOutputPath { get { return Application.dataPath + "/../AssetBundle/" + TargetData.ModuleName + "/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/"; } }

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
            AllAssets.Clear();
            AllPrefabs.Clear();

            TargetData = moduleData;
            BuildType = buildType;
            Version = version;
            UpdateNotice = notice;
            FileHelper.DeletFolder(BundleOutputPath);
            Directory.CreateDirectory(BundleOutputPath);
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
                
                foreach (var filePath in package.Prefabs)
                {
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
            WriteAssetBundleConfig();
            AssetDatabase.Refresh();
            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(BundleOutputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            if (manifest == null)
            {
                EditorUtility.DisplayDialog("Build Asset Bundle", "Build failed!", "Confirm");
                Debug.LogError("GC Asset Framework: AssetBundle Build failed!");
            }
            else
            {
                Debug.Log("GC Asset Framework: Build Scccess!");
                DeleteAllBundleManifestFile();
                EncryptAllBundle();
            }
            ModifyAllFileBundleName(true);
        }

        /// <summary>
        /// 生成AssetBundle 配置文件
        /// </summary>
        private static void WriteAssetBundleConfig()
        {
            BundleConfig config = new BundleConfig();
            config.BundleInfoList = new List<BundleInfo>();
            // 所有AssetBundle文件字典 路径 - BundleName
            Dictionary<string, string> AllBundleFileDic = new Dictionary<string, string>();
        
            string[] allBundleArr = AssetDatabase.GetAllAssetBundleNames();

            foreach (var item in allBundleArr)
            {
                string[] bundleFileArr = AssetDatabase.GetAssetPathsFromAssetBundle(item);
                foreach (var filePath in bundleFileArr)
                {
                    if (!filePath.EndsWith(".cs"))
                    {
                        AllBundleFileDic.Add(filePath, item);
                    }
                }
            }
            // 计算AssetBundle数据，生成AssetBundle配置文件
            foreach (var item in AllBundleFileDic)
            {
                string filePath = item.Key;
                if (!filePath.EndsWith(".cs"))
                {
                    BundleInfo bundleInfo = new BundleInfo();
                    bundleInfo.Path = filePath;
                    bundleInfo.BundleName = item.Value;
                    bundleInfo.AssetName = Path.GetFileName(filePath);
                    bundleInfo.crc = Crc32.GetCrc32(filePath);
                    bundleInfo.BundleDependce = new List<string>();
                    string[] depencies = AssetDatabase.GetDependencies(filePath);
                    foreach (var depence in depencies)
                    {
                        if (!depence.Equals(filePath) && depence.EndsWith(".cs"))
                        {
                            string assetBundleName = "";
                            if (AllBundleFileDic.TryGetValue(depence, out assetBundleName))
                            {
                                if (!bundleInfo.BundleDependce.Contains(assetBundleName))
                                {
                                    bundleInfo.BundleDependce.Add(assetBundleName);
                                }
                            }
                        }
                    }
                    config.BundleInfoList.Add(bundleInfo);
                }
            }
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            string bundleConfigPath = Application.dataPath + "/" + TargetData.ModuleName + "AssetBundleConfig.json"; 
            StreamWriter writer = File.CreateText(bundleConfigPath);
            writer.Write(json);
            writer.Dispose();
            writer.Close();

            AssetImporter importer = AssetImporter.GetAtPath(bundleConfigPath.Replace(Application.dataPath, "Assets"));
            if (importer != null)
            {
                importer.assetBundleName = TargetData.ModuleName + "assetbundleconfig";
            }
        }

        /// <summary>
        /// 进行打包名称
        /// </summary>
        private static void ModifyAllFileBundleName(bool clear = false)
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
            
            if (clear)
            {
                AssetDatabase.RemoveUnusedAssetBundleNames();
                string bundleConfigPath = Application.dataPath + "/" + TargetData.ModuleName + "AssetBundleConfig.json"; 
                AssetImporter close = AssetImporter.GetAtPath(bundleConfigPath.Replace(Application.dataPath, "Assets"));
                if (close != null)
                {
                    close.assetBundleName = "";
                }
            }
        }

        /// <summary>
        /// 确定路径是否为冗余文件
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        private static bool IsRequired(string Path)
        {
            if (Path.EndsWith(".cs")) return true;
            
            foreach (var filePath in AllFilePath)
            {
                if (string.Equals(filePath, Path))
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

        /// <summary>
        /// 删除所有由AssetBundle生成的清单文件
        /// </summary>
        private static void DeleteAllBundleManifestFile()
        {
            string[] fileArr = Directory.GetFiles(BundleOutputPath);
            foreach (var file in fileArr)
            {
                if (file.EndsWith(".manifest"))
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// 加密所有的AssetBundle
        /// </summary>
        private static void EncryptAllBundle()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(BundleOutputPath);
            FileInfo[] fileInfoArr = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int  i = 0; i < fileInfoArr.Length; i ++)
            {
                AES.AESFileEncrypt(fileInfoArr[i].FullName, "gamecrafter");
            }
            Debug.Log("GC Asset Framework: AssetBundle Encrypt Finsish!");
        }
    }
}
