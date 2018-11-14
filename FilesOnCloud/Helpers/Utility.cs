using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FilesOnCloud.Helpers
{
    public static class Utility
    {
        public static byte[] ReadData(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}