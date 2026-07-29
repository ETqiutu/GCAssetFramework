using UnityEngine;

namespace GC.AssetFramework
{
    [CreateAssetMenu(fileName = "BundleSettings", menuName = "GC/AssetFramework/BundleSettings")]
    public class BundleSettings : ScriptableObject
    {
        #region 单例模式
        private static BundleSettings instance;
        
        public static BundleSettings Instance
        {
            get
            {
                if (instance == null) instance = Resources.Load<BundleSettings>("BundleSettings");
                return instance;
            }
        }
        #endregion

        
    }
}
