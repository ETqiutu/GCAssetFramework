using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GC.AssetFramework
{
    [CreateAssetMenu(fileName = "BuildConfigura", menuName = "GC/AssetFramework/BuildConfigura")]
    public class BuildConfigura : ScriptableObject
    {
        #region 单例模式
        private static BuildConfigura instance;
        public static BuildConfigura Instance
        {
            get
            {
                if (instance == null) instance = AssetDatabase.LoadAssetAtPath<BuildConfigura>("Assets/GCAssetFramework/Data/BuildConfigura.asset");
                return instance;
            }
        }
        #endregion
        
        /// <summary>
        /// 所有模块的列表
        /// </summary>
        /// <typeparam name="ModuleData"></typeparam>
        /// <returns></returns>
        public List<ModuleData> AssetsData = new List<ModuleData>();


        /// <summary>
        /// 使用模块名取得模块
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ModuleData GetModuleData(string name)
        {
            foreach (var item in AssetsData)
            {
                if (item.ModuleName == name)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 使用模块名称移除模块
        /// </summary>
        /// <param name="name"></param>
        public void RemoveModuleData(string name)
        {
            for (int i = 0; i < AssetsData.Count; i++)
            {
                if (AssetsData[i].ModuleName == name)
                {
                    AssetsData.Remove(AssetsData[i]);
                    break;
                }
            } 
        }

        /// <summary>
        /// 保存模块数据
        /// </summary>
        /// <param name="moduleData"></param>
        public void SaveModuleData(ModuleData moduleData)
        {
            if (AssetsData.Contains(moduleData))
            {
                for (int i = 0; i < AssetsData.Count; i++)
                {
                    if (AssetsData[i] == moduleData)
                    {
                        AssetsData[i] = moduleData;
                        break;
                    }
                }
            }
            else
            {
                AssetsData.Add(moduleData);
            }
        
            Save();
        }
        
        /// <summary>
        /// 保存数据
        /// </summary>
        public void Save()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
#endif
        }
    }
}
