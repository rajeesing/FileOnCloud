using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesOnCloud.Handlers
{
    public interface ICloudFileHandler
    {
        Task Upload(string folder, string file, byte[] content);
        Task<Stream> Download(string folder, string file);
    }
}
