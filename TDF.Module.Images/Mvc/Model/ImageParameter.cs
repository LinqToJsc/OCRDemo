using System;
using TDF.Module.Images.Enums;

namespace TDF.Module.Images.Mvc.Model
{
    public class ImageParameter
    {
        public BusinessType BusinessType { get; set; }

        public int YearMonth { get; set; }

        public string Id { get; set; }

        public ImageSize Size { get; set; }

        public string Format { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public ImageFormat ImageFormat { get; set; }

        public Guid Guid { get; set; }

        public ImageParameter GetFixed(IImageParameterFixer fixer)
        {
            return fixer.Fix(this);
        }

        public string GetRelativePath()
        {
            var folder = $"\\storage\\images\\{this.Year}\\{this.Month}\\{this.Id}\\";
            if (this.ImageFormat == ImageFormat.Gif)
            {
                return folder + "Full.gif";
            }
            return folder + $"{this.Size}.{this.Format}";
        }
    }
}
