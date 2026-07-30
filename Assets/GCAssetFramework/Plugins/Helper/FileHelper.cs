using System.IO;

namespace GC.AssetsFramework
{
    public static class FileHelper
    {
        public static void Write(string path, byte[] data)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            File.Create(path);
            File.WriteAllBytes(path, data);
        }

        public static void DeletePath(string path)
        {
            if (Directory.Exists(path))
            {
                string[] filePaths = Directory.GetFiles(path);
                foreach (var file in filePaths)
                {
                    if (File.Exists("file"))
                        File.Delete(file);
                }
                Directory.Delete(path);
            }
        }
    }
}