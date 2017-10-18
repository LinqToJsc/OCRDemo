using System;
using TDF.Module.Images.Enums;

namespace TDF.Module.Images.Mvc.Model
{
    public class DefaultImageParameterFixer : IImageParameterFixer
    {
        public ImageParameter Fix(ImageParameter parameter)
        {
            parameter.Guid = Guid.Parse(parameter.Id);
            parameter.Year = parameter.YearMonth/100;
            parameter.Month = parameter.YearMonth%100;
            if (parameter.Year < 2017 || parameter.Year > DateTime.Now.Year + 1 || parameter.Month < 1 ||
                parameter.Month > 12)
            {
                throw new Exception("错误的年月格式");
            }
            parameter.ImageFormat = ImageFormatHelper.GetFormatFromExtension("." + parameter.Format);
            return parameter;
        }
    }
}
