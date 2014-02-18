using System.IO;
using System.Threading.Tasks;

namespace Commonality.Main.FileSystem
{
    public class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public Stream OpenWrite(string path)
        {
            return File.OpenWrite(path);
        }

        public void CreateEmptyFile(string path)
        {
            File.Create(path).Dispose();
        }

        public string GetTempPath()
        {
            return Path.GetTempPath();
        }

        public string GetTempFileName()
        {
            return Path.GetTempFileName();
        }

        public string ReadAllText(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public void WriteAllText(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        public async Task WriteAllTextAsync(string filePath, string text)
        {
            using (var writer = new StreamWriter(filePath))
            {
                await writer.WriteAsync(text);
            }
        }

        public void Copy(string pathFrom, string pathTo)
        {
            File.Copy(pathFrom, pathTo);
        }
    }
}