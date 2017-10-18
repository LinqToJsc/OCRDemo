using System;
using TDF.Core.Exceptions;

namespace TDF.Module.Images.Enums
{
    [Flags]
    public enum ImageFormat
    {
        Jpeg = 1,
        Png = 2,
        Gif = 4,
        Bmp = 8
    }

    public static class ImageFormatHelper
    {
        public static ImageFormat GetFormatFromExtension(string ext)
        {
            if (string.IsNullOrEmpty(ext))
            {
                throw new KnownException("错误的文件格式1");
            }
            ext = ext.ToLower();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                default:
                    throw new KnownException("错误的文件格式2");
            }
        }

        public static string GetExtension(this ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Png:
                    return ".png";
                case ImageFormat.Bmp:
                    return ".bmp";
                case ImageFormat.Gif:
                    return ".gif";
                case ImageFormat.Jpeg:
                default:
                    return ".jpg";
            }
        }

        public static System.Drawing.Imaging.ImageFormat GetSystemImageFormat(this ImageFormat format)
        {
            switch (format)
            {
                case ImageFormat.Png:
                    return System.Drawing.Imaging.ImageFormat.Png;
                case ImageFormat.Bmp:
                    return System.Drawing.Imaging.ImageFormat.Bmp;
                case ImageFormat.Gif:
                    return System.Drawing.Imaging.ImageFormat.Gif;
                case ImageFormat.Jpeg:
                default:
                    return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
        }
    }
}
