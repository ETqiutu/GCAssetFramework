using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GCAssetFramework
{
    [CreateAssetMenu(fileName = "BuildBundleConfigura", menuName = "GC/AssetBuild/BuildBundleConfigura")]
    public class BuildBundleConfigura : ScriptableObject
    {
        #region 单例模式
        private static BuildBundleConfigura instance;

        public static BuildBundleConfigura Instance
        {
            get
            {
                if (instance == null)
                    instance = AssetDatabase.LoadAssetAtPath<BuildBundleConfigura>("Assets/GCAssetFramework/Config/BuildBundleConfigura.asset");
                return instance;
            }
        }
        #endregion
         
        /// <summary>
        /// 资源列表
        /// </summary> 
        [SerializeField]
        public List<BundleModuleData> AssetBundleConfg = new List<BundleModuleData>();

        /// <summary>
        /// 通过模块名称获取模块数据
        /// </summary>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public BundleModuleData GetBundleDataByName(string moduleName)
        {
            foreach (var item in AssetBundleConfg)
            {
                if (item.moduleName == moduleName)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过模块名称移除模块数据
        /// </summary>
        /// <param name="moduleName"></param>
        public void RemoveBundleModuleByName(string moduleName)
        {
            for (int i = 0; i < AssetBundleConfg.Count; i ++)
            {
                if (AssetBundleConfg[i].moduleName == moduleName)
                {
                    AssetBundleConfg.Remove(AssetBundleConfg[i]);
                    break;
                }
            }
        }

        /// <summary>
        /// 存储新的模块资源
        /// </summary>
        /// <param name="bundleModuleData"></param>
        public void SaveModuleData(BundleModuleData bundleModuleData)
        {
            AssetBundleConfg.Add(bundleModuleData);
            Save();
        }

        /// <summary>
        /// 存储
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