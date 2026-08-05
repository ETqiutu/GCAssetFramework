#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace GC.AssetsFramework
{
    public static class AssetBuilder
    {
        public static BuildSetting BuildSetting;
        public static string Output = "";
        public static string ResourcesPath = Application.dataPath + "/Resources/";
        public static string ModuleName;
        private static List<string> AllFiles = new List<string>();
        private static Dictionary<string, List<string>> BundlePaths = new Dictionary<string, List<string>>();

        public static void Initialize(ModuleData moduleData, BuildSetting buildSetting)
        {
            if (moduleData != null && 
                string.IsNullOrEmpty(moduleData.ModuleName) && 
                moduleData.Version != null && 
                moduleData.BundleList != null && 
                moduleData.BundleList.Length <= 0)
                return;
            BuildSetting = buildSetting;
            ModuleName = moduleData.ModuleName;
            string platformName = BuildSetting.BuildPlatform.ToString().ToLower();
            Output = Application.dataPath + "/../AssetBundle/" + platformName + "/" + ModuleName + "/" + moduleData.Version.ToString() + "/";
            if (!Directory.Exists(Output))
            {
                FileHelper.DeletePath(Output);
                Directory.CreateDirectory(Output);
            }
            
            foreach (var bundle in moduleData.BundleList)
            {
                if (BundlePaths.ContainsKey(moduleData.ModuleName + "_" + bundle.BundleName))
                {
                    Debug.LogError("[GC Asset Framework]: Duplicate AssetBundle packages!");
                    continue;
                }
                List<string> paths = new List<string>();
                foreach (var path in bundle.AssetsPath)
                {
                    string realPath = path.Replace(@"\", "/");
                    if (IsRepeatBundleFiles(realPath))
                    {
                        paths.Add(path);
                        AllFiles.Add(path);
                    }
                    else
                        Debug.LogWarning("[GC Asset Framework]: The current module contains duplicate file entries.");
                }
                BundlePaths.Add(moduleData.ModuleName + "_" + bundle.BundleName, paths);
            }
        }

        public static void Build()
        {
            ModifyAllFileBundleName();
            WriteAssetBundleConfig();
            AssetDatabase.Refresh();
            AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(Output, Utilities.GetBundleOptions(BuildSetting.BuildOption), Utilities.GetBuildTarget(BuildSetting.BuildPlatform));
            if (manifest == null)
            {
                Debug.LogError("[GC Asset Framework]: Packaging failed.");
            }
            else
            {
                Debug.Log("[GC Asset Framework]: Packaging successfully.");
                DeleteManifest();
                EncryptAllBundle();
            }
            ModifyAllFileBundleName(true);
        }

        /// <summary>
        /// Modify all file asset bundle name
        /// </summary>
        /// <param name="Clear"></param>
        private static void ModifyAllFileBundleName(bool Clear = false)
        {
            int i = 0;
            foreach (var item in BundlePaths)
            {
                i ++;
                foreach (var path in item.Value)
                {
                    AssetImporter importer = AssetImporter.GetAtPath(path);
                    if (importer != null)
                    {
                        importer.assetBundleName = Clear ? "" : item.Key;
                    }
                }
            }
            if (Clear)
            {
                string bundleConfigPath = Application.dataPath + "/" + ModuleName.ToLower() + "assetbundleconfig.json";
                AssetImporter importer = AssetImporter.GetAtPath(bundleConfigPath.Replace(Application.dataPath, "Assets"));
                if (importer != null)
                {
                    importer.assetBundleName = "";
                }
                AssetDatabase.RemoveUnusedAssetBundleNames();
            }
        }

        /// <summary>
        /// Build manifest;
        /// </summary> 
        private static void WriteAssetBundleConfig()
        {
            BundleConfig config = new BundleConfig();
            config.BundleInfoList = new List<BundleInfo>();

            Dictionary<string, string> AllBundleFilePathDic = new Dictionary<string, string>();
            string[] allbundle = AssetDatabase.GetAllAssetBundleNames();
            foreach (var bundle in allbundle)
            {
                string[] bundleFiles = AssetDatabase.GetAssetPathsFromAssetBundle(bundle);
                foreach (var file in bundleFiles)
                {
                    if (!file.EndsWith(".cs"))
                    {
                        AllBundleFilePathDic.Add(file, bundle);

                    }
                }
            }
            foreach (var item in AllBundleFilePathDic)
            {
                string filePath = item.Key;
                if (filePath.EndsWith(".cs"))
                {
                    BundleInfo bundleInfo = new BundleInfo();
                    bundleInfo.Path = filePath;
                    bundleInfo.BundleName = item.Value;
                    bundleInfo.AssetName = Path.GetFileName(filePath);
                    bundleInfo.CRC = Crc32.GetCrc32(filePath);
                    bundleInfo.Dependencies = new List<string>();

                    string[] dependencies = AssetDatabase.GetDependencies(filePath);
                    foreach (var dependence in dependencies)
                    {
                        if (!dependence.Equals(filePath) && !dependence.EndsWith(".cs"))
                        {
                            string assetBundleName = "";
                            if (AllBundleFilePathDic.TryGetValue(dependence, out assetBundleName))
                            {
                                if (!bundleInfo.Dependencies.Contains(assetBundleName))
                                {
                                    bundleInfo.Dependencies.Add(assetBundleName);
                                }
                            }
                        }
                    }
                    config.BundleInfoList.Add(bundleInfo);
                }
            }
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            string bundleConfigPath = Application.dataPath + "/" + ModuleName.ToLower() + "assetbundleconfig.json";
            StreamWriter writer = File.CreateText(bundleConfigPath);
            writer.Write(json);
            writer.Dispose();
            writer.Close();
            AssetImporter importer = AssetImporter.GetAtPath(bundleConfigPath.Replace(Application.dataPath, "Assets"));
            if (importer != null)
            {
                importer.assetBundleName = ModuleName.ToLower() + "bundleconfig";
            }
        }

        /// <summary>
        /// Check if the path is valid.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsRepeatBundleFiles(string path)
        {
            foreach (var item in AllFiles)
            {
                if (string.Equals(item, path) || path.EndsWith(".cs"))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Delete all manifest files
        /// </summary> 
        private static void DeleteManifest()
        {
            string[] files = Directory.GetFiles(Output);
            foreach (var file in files)
            {
                if (file.EndsWith(".manifest"))
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Encrypt AssetBundle packages
        /// </summary>
        private static void EncryptAllBundle()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Output);
            FileInfo[] fileInfoArr = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            for (int i = 0; i < fileInfoArr.Length; i ++)
            {
                AES.AESEncrypt(fileInfoArr[i].FullName, BuildSetting.EncrypKey);
            }
            Debug.Log("[GC Asset Framework] Encryption successful.");
        }

        
        public static void CopyBundleToStreamingAssets(ModuleData moduleData, bool showTips = true)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Output);
            FileInfo[] fileInfoArr = directoryInfo.GetFiles("*", SearchOption.AllDirectories);
            string streamingAssetsPath = Application.streamingAssetsPath + "/AssetBundle/" + moduleData.ModuleName + "/";
            FileHelper.DeletePath(streamingAssetsPath);

            List<BuildBundleInfo> buildBundleInfos = new List<BuildBundleInfo>();
            Directory.CreateDirectory(streamingAssetsPath);
            for (int i = 0; i < fileInfoArr.Length; i++)
            {
                File.Copy(fileInfoArr[i].FullName, streamingAssetsPath + fileInfoArr[i].Name);
                BuildBundleInfo buildBundleInfo = new BuildBundleInfo();
                buildBundleInfo.FileName = fileInfoArr[i].Name;
                buildBundleInfo.MD5 = MD5.GetMd5FromFile(fileInfoArr[i].FullName);
                buildBundleInfo.Size = fileInfoArr[i].Length / 1024;
                buildBundleInfos.Add(buildBundleInfo);
            }

            string json = JsonConvert.SerializeObject(buildBundleInfos);
            FileHelper.Write(ResourcesPath + ModuleName + "Info.json", Encoding.UTF8.GetBytes(json));
            AssetDatabase.Refresh();
            if (showTips)
            {
                EditorUtility.DisplayDialog(
                    "Inline operation", 
                    $"Embedded resources completed : {ResourcesPath + ModuleName + "Info.json"}", 
                    "Confirm");
            }
            Debug.Log("[GC Asset Framework]: Successfully copied assets to the StreamingAssets directory.");
        }
    }
}
#endif