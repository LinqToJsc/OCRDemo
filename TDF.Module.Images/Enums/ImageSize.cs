using System.Drawing;

namespace TDF.Module.Images.Enums
{
    public enum ImageSize
    {
        Full = 0,
        S80X80 = 1,
        S150X50 = 2,
        S100X100 = 3,
        S150X150 = 4,
        S190X190 = 6,
        H100 = 5
    }

    public static class ImageSizeExtensions
    {
        public static Size GetSize(this ImageSize size)
        {
            switch (size)
            {
                case ImageSize.S80X80:
                    return new Size(80, 80);
                case ImageSize.S100X100:
                    return new Size(100, 100);
                case ImageSize.S150X50:
                    return new Size(150, 50);
                case ImageSize.S150X150:
                    return new Size(150, 150);
                case ImageSize.H100:
                    return new Size(0, 100);
                default:
                case ImageSize.Full:
                    return new Size();
            }
        }
    }
}
