namespace GC.AssetsFramework
{
    /// <summary>
    /// 版本封装
    /// </summary>
    [System.Serializable]
    public class Version
    {
        /// <summary>
        /// 主版本：大功能更新
        /// </summary> 
        public int Major;

        /// <summary>
        /// 次版本：新功能上线
        /// </summary>
        public int Minor;

        /// <summary>
        /// 修订版本:Bug修复
        /// </summary>
        public int Patch;

        /// <summary>
        /// 热更版本：资源/配置更新
        /// </summary>
        public int Hotfix;

        /// <summary>
        /// 构造版本
        /// </summary>
        /// <param name="version"></param>
        public Version(string version)
        {
            var parts = version.Split('.');
            if (parts.Length == 4)
            {
                Major = int.Parse(parts[0]);
                Minor = int.Parse(parts[1]);
                Patch = int.Parse(parts[2]);
                Hotfix = int.Parse(parts[3]);
            }
        }

        /// <summary>
        /// 检查是否为主要更新
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsMajorUpdate(Version other)
        {
            return Major != other.Major || Minor != other.Minor || Patch != other.Patch;
        }
        
        /// <summary>
        /// 检查是否为热更新
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns> 
        public bool IsHotfixUpdate(Version other)
        {
            return Hotfix != other.Hotfix && !IsMajorUpdate(other);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}.{Hotfix}";
        }
    }
}
