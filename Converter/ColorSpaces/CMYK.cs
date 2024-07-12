using System;
using System.Text.RegularExpressions;

namespace ColorUtil.Converter.ColorSpaces
{
    public class CMYK
    {
        public string Name { get; set; } = "CMYK";
        public int? C { get; set; }
        public int? M { get; set; }
        public int? Y { get; set; }
        public int? K { get; set; }
        public string? Code { get; set; }
        public const string Pattern = @"^\s*cmyk\s*\(\s*\d{1,3}%?\s*,\s*\d{1,3}%?\s*,\s*\d{1,3}%?\s*,\s*\d{1,3}%?\s*\)\s*$";

        // Convert RGB to CMYK
        public CMYK From(RGB rgb)
        {
            double r = (rgb.R ?? 0) / 255.0;
            double g = (rgb.G ?? 0) / 255.0;
            double b = (rgb.B ?? 0) / 255.0;

            double k = 1 - Math.Max(Math.Max(r, g), b);
            double c = 0, m = 0, y = 0;

            if (k < 1)
            {
                c = (1 - r - k) / (1 - k);
                m = (1 - g - k) / (1 - k);
                y = (1 - b - k) / (1 - k);
            }

            CMYK cmyk = new()
            {
                C = (int?)Math.Round(c * 100),
                M = (int?)Math.Round(m * 100),
                Y = (int?)Math.Round(y * 100),
                K = (int?)Math.Round(k * 100)
            };

            // Handle edge case for black
            if (rgb.R == 0 && rgb.G == 0 && rgb.B == 0)
            {
                cmyk.C = 0;
                cmyk.M = 0;
                cmyk.Y = 0;
                cmyk.K = 100;
            }

            cmyk.Code = $"cmyk({cmyk.C:F0}%, {cmyk.M:F0}%, {cmyk.Y:F0}%, {cmyk.K:F0}%)";

            return cmyk;
        }

        // Convert CMYK to RGB
        public RGB To(string color)
        {
            // Validate the input using regex pattern
            if (!Regex.IsMatch(color, Pattern))
            {
                throw new ArgumentException("Invalid CMYK color format.");
            }

            // remove all the spaces
            color = color.Replace(" ", "");

            // Remove the "cmyk(" and ")" from the string, accounting for optional spaces
            color = color.Trim().Substring(5).TrimEnd(')').Trim();



            // Split the string into an array.
            string[] cmykArray = color.Split(',');

            // Parse the string values to double
            double c = double.Parse(cmykArray[0].TrimEnd('%')) / 100.0;
            double m = double.Parse(cmykArray[1].TrimEnd('%')) / 100.0;
            double y = double.Parse(cmykArray[2].TrimEnd('%')) / 100.0;
            double k = double.Parse(cmykArray[3].TrimEnd('%')) / 100.0;

            // Convert the CMYK color to RGB.
            RGB rgb = new()
            {
                R = (int)Math.Round(255 * (1 - c) * (1 - k)),
                G = (int)Math.Round(255 * (1 - m) * (1 - k)),
                B = (int)Math.Round(255 * (1 - y) * (1 - k))
            };

            rgb.Code = $"rgb({rgb.R}, {rgb.G}, {rgb.B})";

            return rgb;
        }
    }

}
