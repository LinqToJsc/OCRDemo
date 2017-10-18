using System;
using System.IO;
using System.Linq;

namespace TDF.Core.Models
{
    /// <summary>
    /// 用于接收Post的文件
    /// </summary>
    public class PostedFile
    {
        private byte[] _fileBytes;

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public Stream FileStream { get; set; }

        public long ContentLength { get; set; }

        public byte[] FileBytes
        {
            get
            {
                if (_fileBytes == null && FileStream != null)
                {
                    try
                    {
                        _fileBytes = new byte[FileStream.Length];
                        FileStream.Read(_fileBytes, 0, _fileBytes.Count());
                        FileStream.Flush();
                    }
                    catch (Exception)
                    {
                    }

                }
                return _fileBytes;
            }
            set { _fileBytes = value; }
        }

        public string Suffixs
        {
            get
            {
                var extension = Path.GetExtension(FileName);
                if (extension != null) return extension.Substring(1).ToLower();
                return string.Empty;
            }
        }
    }

}
