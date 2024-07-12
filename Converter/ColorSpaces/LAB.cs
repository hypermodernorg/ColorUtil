using System;
using System.Text.RegularExpressions;

namespace ColorUtil.Converter.ColorSpaces
{
    // class for the LAB colorspace
    public class LAB
    {
        public string Name { get; set; } = "LAB";
        public double? L { get; set; }
        public double? A { get; set; }
        public double? B { get; set; }
        public string? Code { get; set; }
        public const string Pattern = @"^\s*lab\s*\(\s*-?\d{1,3}\s*,\s*-?\d{1,3}\s*,\s*-?\d{1,3}\s*\)\s*$";

        // Convert RGB to LAB
        public LAB From(RGB rgb)
        {
            double r = (rgb.R ?? 0) / 255.0;
            double g = (rgb.G ?? 0) / 255.0;
            double b = (rgb.B ?? 0) / 255.0;

            r = r > 0.04045 ? Math.Pow((r + 0.055) / 1.055, 2.4) : r / 12.92;
            g = g > 0.04045 ? Math.Pow((g + 0.055) / 1.055, 2.4) : g / 12.92;
            b = b > 0.04045 ? Math.Pow((b + 0.055) / 1.055, 2.4) : b / 12.92;

            r *= 100.0;
            g *= 100.0;
            b *= 100.0;

            double x = r * 0.4124 + g * 0.3576 + b * 0.1805;
            double y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            // Convert XYZ to LAB
            x /= 95.047;
            y /= 100.0;
            z /= 108.883;

            x = x > 0.008856 ? Math.Pow(x, 1.0 / 3.0) : 7.787 * x + 16.0 / 116.0;
            y = y > 0.008856 ? Math.Pow(y, 1.0 / 3.0) : 7.787 * y + 16.0 / 116.0;
            z = z > 0.008856 ? Math.Pow(z, 1.0 / 3.0) : 7.787 * z + 16.0 / 116.0;

            LAB lab = new LAB
            {
                L = 116.0 * y - 16.0,
                A = 500.0 * (x - y),
                B = 200.0 * (y - z),
                Code = $"lab({116.0 * y - 16.0:F0}, {500.0 * (x - y):F0}, {200.0 * (y - z):F0})"
            };

            return lab;
        }

        // Convert LAB to RGB
        public RGB To(string lab)
        {
            // Validate the input using regex pattern
            if (!Regex.IsMatch(lab, Pattern))
            {
                throw new ArgumentException("Invalid LAB color format.");
            }
            // remove spaces
            lab = lab.Replace(" ", "");

            // Parse the LAB string
            var labValues = lab.Replace("lab(", "").Replace(")", "").Split(',');
            double L = double.Parse(labValues[0].Trim());
            double a = double.Parse(labValues[1].Trim());
            double b = double.Parse(labValues[2].Trim());

            // Convert LAB to XYZ
            double y = (L + 16.0) / 116.0;
            double x = a / 500.0 + y;
            double z = y - b / 200.0;

            double x3 = Math.Pow(x, 3.0);
            double y3 = Math.Pow(y, 3.0);
            double z3 = Math.Pow(z, 3.0);

            x = x3 > 0.008856 ? x3 : (x - 16.0 / 116.0) / 7.787;
            y = y3 > 0.008856 ? y3 : (y - 16.0 / 116.0) / 7.787;
            z = z3 > 0.008856 ? z3 : (z - 16.0 / 116.0) / 7.787;

            x *= 95.047;
            y *= 100.0;
            z *= 108.883;

            // Convert XYZ to RGB
            x /= 100.0;
            y /= 100.0;
            z /= 100.0;

            double r = x * 3.2406 + y * -1.5372 + z * -0.4986;
            double g = x * -0.9689 + y * 1.8758 + z * 0.0415;
            double b2 = x * 0.0557 + y * -0.2040 + z * 1.0570;

            r = r > 0.0031308 ? 1.055 * Math.Pow(r, 1 / 2.4) - 0.055 : 12.92 * r;
            g = g > 0.0031308 ? 1.055 * Math.Pow(g, 1 / 2.4) - 0.055 : 12.92 * g;
            b2 = b2 > 0.0031308 ? 1.055 * Math.Pow(b2, 1 / 2.4) - 0.055 : 12.92 * b2;

            RGB rgb = new RGB
            {
                R = (int)Math.Max(0, Math.Min(255, r * 255)),
                G = (int)Math.Max(0, Math.Min(255, g * 255)),
                B = (int)Math.Max(0, Math.Min(255, b2 * 255)),
                Code = $"rgb({(int)Math.Max(0, Math.Min(255, r * 255))}, {(int)Math.Max(0, Math.Min(255, g * 255))}, {(int)Math.Max(0, Math.Min(255, b2 * 255))})"
            };

            return rgb;
        }
    }

}