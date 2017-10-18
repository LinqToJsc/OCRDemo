using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;
using TDF.Module.Images.Enums;
using Encoder = System.Drawing.Imaging.Encoder;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace TDF.Module.Images.Extensions
{
    public static class ImageExtensions
    {
        public static Image ToSize(this Image image, Size size)
        {
            var newSize = image.Size.ResizeTo(size);

            var bitmap = new Bitmap(newSize.Width, newSize.Height);
            var g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.DrawImage(image, 0, 0, newSize.Width, newSize.Height);
            g.Dispose();
            return bitmap;
        }

        public static void SaveInFormat(this Image image, string path)
        {
            image.Save(path, GetImageFormat(Path.GetExtension(path)));
        }

        /// <summary>
        /// .png, .gif etc
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static ImageFormat GetImageFormat(this string extension)
        {
            switch (extension.ToLower())
            {
                case ".png":
                    return ImageFormat.Png;
                case ".gif":
                    return ImageFormat.Gif;
                case ".ico":
                    return ImageFormat.Icon;
                case ".bmp":
                    return ImageFormat.Bmp;
            }
            return ImageFormat.Jpeg;
        }

        public static void SaveToFileInQuality(this Image image, string path, ImageFormat format)
        {
            var parameters = new EncoderParameters();
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, new long[] { 90 });
            var encoder = ImageCodecInfo.GetImageEncoders().First(x => x.FormatID == format.Guid);
            image.Save(path, encoder, parameters);
        }

        public static Entity.Image ToImage(this PostedFile file)
        {
            Enums.ImageFormat imageFormat;
            switch (file.Suffixs)
            {
                case "jpg":
                    imageFormat = Enums.ImageFormat.Jpeg;
                    break;
                case "gif":
                    imageFormat = Enums.ImageFormat.Gif;
                    break;
                case "bmp":
                    imageFormat = Enums.ImageFormat.Bmp;
                    break;
                case "png":
                    imageFormat = Enums.ImageFormat.Png;
                    break;
                default:
                    throw new Exception("此文件不是图片类型");
                    break;
            }
            var image = new Entity.Image()
            {
                Format = imageFormat,
                Length = file.ContentLength,
                Bytes = file.FileBytes,
                CreateTime = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = file.FileName,
                BusinessType = BusinessType.Default
            };
            using (var sysImage = Image.FromStream(file.FileStream))
            {
                image.Width = sysImage.Width;
                image.Height = sysImage.Height;
            }
            return image;
        }
    }
}
