using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Text;

namespace GC.AssetsFramework
{
    public class VersionControl
    {
        // 版本信息存储路径
        private static string VersionFilePath => Path.Combine(Application.persistentDataPath, "version.txt");
        
        // 资源清单存储路径
        private static string ManifestFilePath => Path.Combine(Application.persistentDataPath, "manifest.json");

        // 取得本地版本
        public static Version GetLocalVersion()
        {
            try
            {
                if (File.Exists(VersionFilePath))
                {
                    string versionStr = File.ReadAllText(VersionFilePath);
                    return new Version(versionStr);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[GC Assets] Version load error!: {e.Message}");
            }
            return new Version("0.0.0.0");
        }

        // 保存本地版本
        public static void SaveLocalVersion(Version version)
        {
            try
            {
                FileHelper.Write(VersionFilePath, Encoding.UTF8.GetBytes(version.ToString()));
                Debug.Log($"[GC Assets] Version save successfully!: {version}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[GC Assets] Version save failed!: {e.Message}");
            }
        }

        // 取得本地资源加载清单
        public static ManifestData GetLocalManifest()
        {
            try
            {
                if (File.Exists(ManifestFilePath))
                {
                    string json = File.ReadAllText(ManifestFilePath);
                    return JsonConvert.DeserializeObject<ManifestData>(json);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[GC Assets] Load local file failed!: {e.Message}");
            }
            return null;
        }

        // 保存本地资源清单
        public static void SaveLocalManifest(ManifestData manifest)
        {
            try
            {
                string json = JsonConvert.SerializeObject(manifest);
                FileHelper.Write(ManifestFilePath, Encoding.UTF8.GetBytes(json));
                Debug.Log($"[GC Assets] Save manifest successfully!");
            }
            catch (Exception e)
            {
                Debug.LogError($"[GC Assets] Save local manifest failed: {e.Message}");
            }
        }
    }
}
