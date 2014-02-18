using System.IO;

namespace Commonality.Main.Extensions
{
    public static class StreamExtensions
    {
        public static string ReadAsStringToEnd(this Stream input)
        {
            using (var reader = new StreamReader(input))
            {
                return reader.ReadToEnd();
            }
        }
    }
}