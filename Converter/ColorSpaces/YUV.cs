using System;
using System.Text.RegularExpressions;

namespace ColorUtil.Converter.ColorSpaces
{
    public class YUV
    {
        public string Name { get; set; } = "YUV";
        public double? Y { get; set; }
        public double? U { get; set; }
        public double? V { get; set; }
        public string? Code { get; set; }
        public const string Pattern = @"^\s*yuv\s*\(\s*([0-9]*\.?[0-9]+(?:[eE][-+]?[0-9]+)?)\s*,\s*([0-9]*\.?[0-9]+(?:[eE][-+]?[0-9]+)?)\s*,\s*([0-9]*\.?[0-9]+(?:[eE][-+]?[0-9]+)?)\s*\)\s*$";

        // Convert RGB to YUV
        public YUV From(RGB rgb)
        {
            double r = (double)(rgb.R / 255.0);
            double g = (double)(rgb.G / 255.0);
            double b = (double)(rgb.B / 255.0);

            YUV yuv = new YUV
            {
                Y = 0.299 * r + 0.587 * g + 0.114 * b,
                U = -0.14713 * r - 0.28886 * g + 0.436 * b,
                V = 0.615 * r - 0.51499 * g - 0.10001 * b
            };

            yuv.Code = $"yuv({yuv.Y.Value:F15}, {yuv.U.Value:F15}, {yuv.V.Value:F15})";
            return yuv;
        }

        // Convert YUV to RGB
        public RGB To(string color)
        {
            var match = Regex.Match(color, Pattern, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid YUV color string format.");
            }

            var yuvValues = color.Replace("yuv", "").Replace(")", "").Replace("(", "").Replace(" ", "").Split(',');
            double y = double.Parse(yuvValues[0].Trim());
            double u = double.Parse(yuvValues[1].Trim());
            double v = double.Parse(yuvValues[2].Trim());

            double r = y + 1.13983 * v;
            double g = y - 0.39465 * u - 0.58060 * v;
            double b = y + 2.03211 * u;

            RGB rgb = new RGB
            {
                R = (int)Math.Max(0, Math.Min(255, r * 255)),
                G = (int)Math.Max(0, Math.Min(255, g * 255)),
                B = (int)Math.Max(0, Math.Min(255, b * 255)),
                Code = $"rgb({(int)Math.Max(0, Math.Min(255, r * 255))}, {(int)Math.Max(0, Math.Min(255, g * 255))}, {(int)Math.Max(0, Math.Min(255, b * 255))})"
            };

            return rgb;
        }
    }
}