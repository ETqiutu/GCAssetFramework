using System.IO;

namespace GC.AssetsFramework
{
    public static class FileHelper
    {
        public static void Write(string path, byte[] data)
        {
            if (File.Exists(path)) File.Delete(path);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            FileStream stream = File.Create(path);
            stream.Write(data, 0, data.Length);
            stream.Dispose();
            stream.Close();
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