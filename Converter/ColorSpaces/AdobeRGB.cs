using System;
using System.Text.RegularExpressions;

namespace ColorUtil.Converter.ColorSpaces
{
    public class AdobeRGB
    {
        public string Name { get; set; } = "Adobe RGB";
        public int? R { get; set; }
        public int? G { get; set; }
        public int? B { get; set; }
        public string? Code { get; set; }
        public const string Pattern = @"^\s*AdobeRGB\s*\(\s*(\d{1,3})\s*,\s*(\d{1,3})\s*,\s*(\d{1,3})\s*\)\s*$";

        // Convert RGB to Adobe RGB
        public AdobeRGB From(RGB rgb)
        {
            double[] linearRgb = ToLinear(rgb);
            double[] adobeRgb = new double[3];

            // Matrix to convert linear sRGB to linear Adobe RGB
            double[,] M = {
                { 0.5767309, 0.1855540, 0.1881852 },
                { 0.2973769, 0.6273491, 0.0752741 },
                { 0.0270343, 0.0706872, 0.9911085 }
            };

            for (int i = 0; i < 3; i++)
            {
                adobeRgb[i] = M[i, 0] * linearRgb[0] + M[i, 1] * linearRgb[1] + M[i, 2] * linearRgb[2];
                adobeRgb[i] = AdobeRGBGammaCorrection(adobeRgb[i]);
            }

            AdobeRGB adobeRgbColor = new()
            {
                R = (int)Math.Round(adobeRgb[0] * 255),
                G = (int)Math.Round(adobeRgb[1] * 255),
                B = (int)Math.Round(adobeRgb[2] * 255)
            };

            adobeRgbColor.Code = $"AdobeRGB({adobeRgbColor.R}, {adobeRgbColor.G}, {adobeRgbColor.B})";
            return adobeRgbColor;
        }

        // Convert Adobe RGB to RGB
        public RGB To(string color)
        {
            // Extract the RGB components
            var match = Regex.Match(color, Pattern);
            if (!match.Success)
            {
                throw new FormatException("Invalid Adobe RGB format");
            }

            double r = double.Parse(match.Groups[1].Value) / 255.0;
            double g = double.Parse(match.Groups[2].Value) / 255.0;
            double b = double.Parse(match.Groups[3].Value) / 255.0;

            // Reverse gamma correction
            r = InverseAdobeRGBGammaCorrection(r);
            g = InverseAdobeRGBGammaCorrection(g);
            b = InverseAdobeRGBGammaCorrection(b);

            double[] adobeRgb = { r, g, b };
            double[] linearRgb = new double[3];

            // Matrix to convert linear Adobe RGB to linear sRGB
            double[,] M = {
                { 2.0413690, -0.5649464, -0.3446944 },
                { -0.9692660, 1.8760108, 0.0415560 },
                { 0.0134474, -0.1183897, 1.0154096 }
            };

            for (int i = 0; i < 3; i++)
            {
                linearRgb[i] = M[i, 0] * adobeRgb[0] + M[i, 1] * adobeRgb[1] + M[i, 2] * adobeRgb[2];
                linearRgb[i] = Math.Max(0, Math.Min(1, linearRgb[i])); // Clamp to [0, 1]
                linearRgb[i] = GammaCorrection(linearRgb[i]); // Apply sRGB gamma correction
            }

            RGB rgb = new()
            {
                R = (int)Math.Round(255 * linearRgb[0]),
                G = (int)Math.Round(255 * linearRgb[1]),
                B = (int)Math.Round(255 * linearRgb[2])
            };

            rgb.Code = $"rgb({rgb.R}, {rgb.G}, {rgb.B})";
            return rgb;
        }

        private double AdobeRGBGammaCorrection(double channel)
        {
            return channel <= 0.018 ? channel * 4.5 : 1.099 * Math.Pow(channel, 1 / 2.19921875) - 0.099;
        }

        private double InverseAdobeRGBGammaCorrection(double channel)
        {
            return channel <= 0.081 ? channel / 4.5 : Math.Pow((channel + 0.099) / 1.099, 2.19921875);
        }

        private double GammaCorrection(double channel)
        {
            return channel <= 0.0031308 ? 12.92 * channel : 1.055 * Math.Pow(channel, 1 / 2.4) - 0.055;
        }

        public double[] ToLinear(RGB rgb)
        {
            double r = (double)rgb.R / 255.0;
            double g = (double)rgb.G / 255.0;
            double b = (double)rgb.B / 255.0;

            r = r <= 0.04045 ? r / 12.92 : Math.Pow((r + 0.055) / 1.055, 2.4);
            g = g <= 0.04045 ? g / 12.92 : Math.Pow((g + 0.055) / 1.055, 2.4);
            b = b <= 0.04045 ? b / 12.92 : Math.Pow((b + 0.055) / 1.055, 2.4);

            return new double[] { r, g, b };
        }
    }


}