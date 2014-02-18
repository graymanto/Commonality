using System.IO;
using System.Threading.Tasks;

namespace Commonality.Main.FileSystem
{
    public interface IFileSystem
    {
        bool FileExists(string path);

        void Delete(string path);

        Stream OpenRead(string path);

        Stream OpenWrite(string path);

        string ReadAllText(string path);

        void CreateEmptyFile(string path);

        string GetTempPath();

        string GetTempFileName();

        void Copy(string pathFrom, string pathTo);

        void WriteAllText(string filePath, string text);

        Task WriteAllTextAsync(string filePath, string text);
    }
}