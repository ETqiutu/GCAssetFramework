using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;

namespace GC.AssetFramework
{
    public class BundleTools
    {
        /// <summary>
        /// 脚本存储路径
        /// </summary>
        public static string BundleModuleEnumFilePath = Application.dataPath + "/GCAssetFramework/Config/BundleModuleEnum.cs";

        [MenuItem("GC/Tool/Generate Module Enum")]
        public static void GenerateBundleModuleEnum()
        {
            string namespaceName = "GC.AssetFramework";
            string name = "BundleModuleEnum";

            if (File.Exists(BundleModuleEnumFilePath))
            {
                File.Create(BundleModuleEnumFilePath);
                AssetDatabase.Refresh();
            }

            var writer = File.CreateText(BundleModuleEnumFilePath);
            writer.WriteLine("/*");
            writer.WriteLine("Title:  AssetBundle模块枚举");
            writer.WriteLine($"Date:  {DateTime.Now}");
            writer.WriteLine("Author: GameCrafter");
            writer.WriteLine("Description: Represents each module which is used to download an load");
            writer.WriteLine("*/");
            writer.WriteLine($"namespace {namespaceName}");
            writer.WriteLine("{");
            List<BundleModuleData> moduleList = BuildBundleConfigura.Instance.AssetBundleConfg;
            if (moduleList == null || moduleList.Count <= 0)
                return;
            writer.WriteLine($"\tpublic enum {name}");
            writer.WriteLine("\t{");
            writer.WriteLine("\t\tNone,");
            for (int i = 0; i < moduleList.Count; i ++)
            {
                writer.WriteLine("\t\t" + moduleList[i].moduleName + ",");
            }
            writer.WriteLine("\t}");
            writer.WriteLine("}");
            writer.Close();
            AssetDatabase.Refresh();
        }
    }
}