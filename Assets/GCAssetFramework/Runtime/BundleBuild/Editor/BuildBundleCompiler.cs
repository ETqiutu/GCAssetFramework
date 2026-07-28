using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEditor;
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
        /// 更新公告
        /// </summary>
        private static string UpdateNotice;
        
        /// <summary>
        /// 热更新补丁版本
        /// </summary>
        private static int HotPatchVersion;
        
        /// <summary>
        /// 打包类型
        /// </summary>
        private static BuildType BuildType;
        
        /// <summary>
        /// 打包模块数据
        /// </summary>
        private static BundleModuleData ModuleData;

        /// <summary>
        /// 所有AssetBundle文件路径列表
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private static List<string> AllBundlePathList = new List<string>();

        /// <summary>
        /// 所有文件夹的Bundle字典
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, List<string>> AllFolderBundleDic = new Dictionary<string, List<string>>();

        /// <summary>
        /// 所有文件夹的Prefab字典
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, List<string>> AllPrefabBundleDic = new Dictionary<string, List<string>>();

        /// <summary>
        /// 打包模块类型
        /// </summary>
        private static BundleModuleEnum ModuleType;

        /// <summary>
        /// 打包AssetBundle
        /// </summary>
        /// <param name="moduleData">资源模块的配置数据</param>
        /// <param name="buildType">构建类型</param>
        /// <param name="hotPathVersion">热更新版本</param>
        /// <param name="updateNotice">热更新公告</param> 
        public static void BuildAssetBundle(BundleModuleData moduleData,BuildType buildType = BuildType.AssetBundle, int hotPathVersion = 0, string updateNotice = "")
        {
            Initialization(moduleData, buildType, hotPathVersion, updateNotice);
            BuildAllFolder();
            BuildRootSubFolder();
            BuildAllPrefabs();
            BuildAllAssetBundle();
        }  

        /// <summary>
        /// 初始化打包设置
        /// </summary>
        /// <param name="moduleData"></param>
        /// <param name="buildType"></param>
        /// <param name="hotPathVersion"></param>
        /// <param name="updateNotice"></param> 
        public static void Initialization(BundleModuleData moduleData,BuildType buildType = BuildType.AssetBundle, int hotPathVersion = 0, string updateNotice = "")
        {
            AllBundlePathList.Clear();
            AllFolderBundleDic.Clear();
            AllPrefabBundleDic.Clear();

            BuildType = buildType;
            UpdateNotice = updateNotice;
            ModuleData = moduleData;
            HotPatchVersion = hotPathVersion;
            ModuleType = (BundleModuleEnum)Enum.Parse(typeof(BundleModuleEnum) ,moduleData.moduleName);
        }

        /// <summary>
        /// 打包所有文件夹AssetBundle
        /// </summary>
        public static void BuildAllFolder()
        {
            if (ModuleData.SignFolderPathArr == null || ModuleData.SignFolderPathArr.Length == 0)
                return;
            
            for (int i = 0; i < ModuleData.SignFolderPathArr.Length; i ++)
            {
                string path = ModuleData.SignFolderPathArr[i].bundlePath.Replace(@"\", "/");
                if (!IsPrepeatBundleFile(path))
                {
                    AllBundlePathList.Add(path);
                    // 获取已模块名 + AbName的格式的AssetBundle包名
                    string bundleName = GenerateBundleName(ModuleData.SignFolderPathArr[i].abName);
                    if (!AllFolderBundleDic.ContainsKey(bundleName))
                    {
                        AllFolderBundleDic.Add(bundleName, new List<string>{ path });
                    }
                    else
                    {
                        AllFolderBundleDic[bundleName].Add(path);
                    }
                }
                else
                {
                    Debug.LogError("RepeatBundleFile: " + path);
                }
            }
        }

        /// <summary>
        /// 打包父文件夹下的所有子文件夹
        /// </summary>
        public static void BuildRootSubFolder()
        {
            if (ModuleData.RootFolderPathArr == null || ModuleData.RootFolderPathArr.Length == 0)
                return;
            
            for (int i = 0; i < ModuleData.RootFolderPathArr.Length; i ++)
            {
                string path = ModuleData.RootFolderPathArr[i] + "/";
                string[] folderArr = Directory.GetDirectories(path);
                foreach (var folder in folderArr)
                {
                    path = folder.Replace(@"\", "/");
                    int nameIndex = path.LastIndexOf("/") + 1;
                    string bundleName = GenerateBundleName(path.Substring(nameIndex, path.Length - nameIndex));
                    if (!IsPrepeatBundleFile(path))
                    {
                        AllBundlePathList.Add(path);
                        if (!AllFolderBundleDic.ContainsKey(bundleName))
                        {
                            AllFolderBundleDic.Add(bundleName, new List<string>{ path });
                        }
                        else
                        {
                            AllFolderBundleDic[bundleName].Add(path);
                        }
                    }
                    else
                    {
                        Debug.LogError("RepeatBundle file FolderPath: " + path);
                    }
                    string[] filePathArr = Directory.GetFiles(path, "*");
                    foreach (var file in filePathArr)
                    {
                        if (!file.EndsWith(".meta"))
                        {
                            string filePath = file.Replace(@"\", "/");
                            if (!IsPrepeatBundleFile(filePath))
                            {
                                AllBundlePathList.Add(filePath);
                                if (!AllFolderBundleDic.ContainsKey(bundleName))
                                {
                                    AllFolderBundleDic.Add(bundleName, new List<string>{ filePath });
                                }
                                else
                                {
                                    AllFolderBundleDic[bundleName].Add(filePath);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 打包指定文件夹下的所有预制体
        /// </summary>
        public static void BuildAllPrefabs()
        {
            if (ModuleData.PrefabPathArr == null || ModuleData.PrefabPathArr.Length == 0)
                return;

            string[] guidArr = AssetDatabase.FindAssets("t:Prefab", ModuleData.PrefabPathArr);
            for (int i = 0; i < guidArr.Length; i ++)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(guidArr[i]);
                string bundlename = GenerateBundleName(Path.GetFileNameWithoutExtension(filePath));
                if (!AllBundlePathList.Contains(filePath))
                {
                    // 获取依赖项
                    string[] dependsArr = AssetDatabase.GetDependencies(filePath);
                    List<string> dependsList = new List<string>();
                    for (int j = 0; j < dependsArr.Length; j ++)
                    {
                        string path = dependsArr[j];
                        if (!IsPrepeatBundleFile(path))
                        {
                            AllBundlePathList.Add(path);
                            dependsList.Add(path);
                        }
                    }
                    if (!AllPrefabBundleDic.ContainsKey(bundlename))
                    {
                        AllPrefabBundleDic.Add(bundlename, dependsList);
                    }
                    else
                    {
                        Debug.LogError("重复的预制体名称! 当前模块下有预制体文件重复 Name:" + bundlename);
                    }
                }
            }
        }

        /// <summary>
        /// 打包AssetBundle
        /// </summary> 
        public static void BuildAllAssetBundle()
        {
            // 修改所有打包文件的AssetBundleName
            ModifyAllFileBundleName();
            // 生成打包AssetBundle配置

            // 调用Unity API 实现打包
        }

        /// <summary>
        /// 修改或者清空AssetBundleName
        /// </summary>
        /// <param name="clear"></param>
        public static void ModifyAllFileBundleName(bool clear = false)
        {
            int i = 0;
            // 修改所有文件夹下的AssetBundle name
            foreach (var item in AllFolderBundleDic)
            {
                i ++;
                EditorUtility.DisplayProgressBar("Modify AssetBundle Name", "Name:" + item.Key, i * 1.0f / AllFolderBundleDic.Count);
                foreach (var path in item.Value)
                {
                    AssetImporter importer = AssetImporter.GetAtPath(path);
                    if (importer != null)
                    {
                        importer.assetBundleName = (clear ? "None" : item.Key + ".unity");
                    }
                }
            }
            i = 0;
            foreach (var item in AllPrefabBundleDic)
            {
                i ++;
                List<string>  bundleList = item.Value;
                foreach (var path in bundleList)
                {
                    EditorUtility.DisplayProgressBar("Modify AssetBundle Name", "Name:" + item.Key, i * 1.0f / AllFolderBundleDic.Count);
                    AssetImporter importer = AssetImporter.GetAtPath(path);
                    if (importer != null)
                    {
                        importer.assetBundleName = (clear ? "None" : item.Key + ".unity");
                    }
                }
            }

            if (clear)
            {
                AssetDatabase.RemoveUnusedAssetBundleNames();
            }
        }

        /// <summary>
        /// 判断是否为冗余包
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPrepeatBundleFile(string path)
        {
            foreach (var item in AllBundlePathList)
            {
                if (string.Equals(item, path) || item.Contains(path) || path.EndsWith(".cs"))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 生成包名称
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public static string GenerateBundleName(string abName)
        {
            return ModuleType.ToString() + "_" + abName;
        }
    }
}
