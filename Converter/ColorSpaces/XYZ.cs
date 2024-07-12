using System;

namespace ColorUtil.Converter.ColorSpaces
{
    public class XYZ
    {
        public string Name { get; set; } = "XYZ";
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public string? Code { get; set; }
        public const string Pattern = @"^\s*xyz\s*\(\s*([0-9]{1,20}(\.[0-9]+)?)\s*,\s*([0-9]{1,20}(\.[0-9]+)?)\s*,\s*([0-9]{1,20}(\.[0-9]+)?)\s*\)\s*$";

        // test
        // Convert RGB to XYZ
        public XYZ From(RGB rgb)
        {
            // Normalize RGB values to the range [0, 1]
            double r = (double)(rgb.R / 255.0);
            double g = (double)(rgb.G / 255.0);
            double b = (double)(rgb.B / 255.0);

            // Apply inverse gamma correction (sRGB)
            r = r > 0.04045 ? Math.Pow((r + 0.055) / 1.055, 2.4) : r / 12.92;
            g = g > 0.04045 ? Math.Pow((g + 0.055) / 1.055, 2.4) : g / 12.92;
            b = b > 0.04045 ? Math.Pow((b + 0.055) / 1.055, 2.4) : b / 12.92;

            // Convert to XYZ using the RGB to XYZ conversion matrix
            double x = r * 0.4124 + g * 0.3576 + b * 0.1805;
            double y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            XYZ xyz = new()
            {
                X = x * 100,
                Y = y * 100,
                Z = z * 100
            };

            xyz.Code = $"xyz({xyz.X}, {xyz.Y}, {xyz.Z})";
            return xyz;
        }

        // Convert XYZ to RGB
        public RGB To(string color)
        {
            try
            {
                // Remove all spaces from the string.
                color = color.Replace(" ", "");
                color = color.Replace("(", "");
                color = color.Replace(")", "");

                // remove xyz regardless of case
                color = color.ToLower().Replace("xyz", "");



                // Split the string into an array.
                string[] xyz = color.Split(',');
                double x = double.Parse(xyz[0]);
                double y = double.Parse(xyz[1]);
                double z = double.Parse(xyz[2]);

                // Normalize for the D65 illuminant
                x /= 100.0;
                y /= 100.0;
                z /= 100.0;

                // Convert to RGB using the XYZ to RGB conversion matrix
                double r = x * 3.2406 + y * -1.5372 + z * -0.4986;
                double g = x * -0.9689 + y * 1.8758 + z * 0.0415;
                double b = x * 0.0557 + y * -0.2040 + z * 1.0570;

                // Apply gamma correction (sRGB)
                r = r > 0.0031308 ? 1.055 * Math.Pow(r, 1 / 2.4) - 0.055 : 12.92 * r;
                g = g > 0.0031308 ? 1.055 * Math.Pow(g, 1 / 2.4) - 0.055 : 12.92 * g;
                b = b > 0.0031308 ? 1.055 * Math.Pow(b, 1 / 2.4) - 0.055 : 12.92 * b;

                // Clamp values to the range [0, 255]
                RGB rgb = new()
                {
                    R = (int)Math.Max(0, Math.Min(255, r * 255)),
                    G = (int)Math.Max(0, Math.Min(255, g * 255)),
                    B = (int)Math.Max(0, Math.Min(255, b * 255))
                };

                rgb.Code = $"rgb({rgb.R}, {rgb.G}, {rgb.B})";
                return rgb;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid XYZ color string format.", ex);
            }
        }
    }
}