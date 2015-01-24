using Ionic.Zip;
using System.IO;

namespace GoogleMapping.Controllers
{
    public class Zippy
    {
        public static byte[] Unzip(byte[] data)
        {
            using (var outputStream = new MemoryStream())
            using (var inputStream = new MemoryStream(data))
            {
                using (var zipInputStream = new ZipInputStream(inputStream))
                {
                    zipInputStream.GetNextEntry();
                    zipInputStream.CopyTo(outputStream);
                }
                return outputStream.ToArray();
            }
        }       
    }
}