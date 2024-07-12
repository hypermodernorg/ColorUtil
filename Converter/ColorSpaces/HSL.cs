namespace ColorUtil.Converter.ColorSpaces
{
    public class HSL
    {
        public string Name { get; set; } = "HSL";
        public int? H { get; set; }
        public int? S { get; set; }
        public int? L { get; set; }
        public string? Code { get; set; }
        public const string Pattern = @"^\s*hsl\s*\(\s*\d{1,3}\s*,\s*\d{1,3}%\s*,\s*\d{1,3}%\s*\)\s*$";

        // Convert RGB to HSL
        public HSL From(RGB rgb)
        {
            double r = (double)rgb.R / 255.0;
            double g = (double)rgb.G / 255.0;
            double b = (double)rgb.B / 255.0;
            double cmax = Math.Max(r, Math.Max(g, b));
            double cmin = Math.Min(r, Math.Min(g, b));
            double delta = cmax - cmin;
            double h = 0;
            double s = 0;
            double l = (cmax + cmin) / 2;

            if (delta != 0)
            {
                if (cmax == r)
                {
                    h = 60 * (((g - b) / delta + 6) % 6);
                }
                else if (cmax == g)
                {
                    h = 60 * ((b - r) / delta + 2);
                }
                else if (cmax == b)
                {
                    h = 60 * ((r - g) / delta + 4);
                }

                s = delta / (1 - Math.Abs(2 * l - 1));
            }

            HSL hsl = new()
            {
                H = (int)Math.Round(h),
                S = (int)Math.Round(s * 100),
                L = (int)Math.Round(l * 100)
            };

            hsl.Code = $"hsl({hsl.H}, {hsl.S}%, {hsl.L}%)";
            return hsl;
        }

        // Convert HSL to RGB
        public RGB To(string color)
        {
            // Convert an HSL string in this format: hsl(0, 0%, 0%) to the class RGB
            // Remove all spaces from the string.
            color = color.Replace(" ", "");

            // Remove the hsl( and ) from the string.
            color = color.Substring(4, color.Length - 5);

            // Split the string into an array.
            string[] hsl = color.Split(',');
            int h = int.Parse(hsl[0]);
            int s = int.Parse(hsl[1].Replace("%", ""));
            int l = int.Parse(hsl[2].Replace("%", ""));
            RGB rgb = new()
            {
                R = 0,
                G = 0,
                B = 0
            };

            // Convert the HSL color to RGB.
            double c = (1 - Math.Abs(2 * l / 100.0 - 1)) * s / 100.0;
            double x = c * (1 - Math.Abs(h / 60.0 % 2 - 1));
            double m = l / 100.0 - c / 2.0;
            double r = 0;
            double g = 0;
            double b = 0;

            if (h >= 0 && h < 60)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (h >= 60 && h < 120)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (h >= 120 && h < 180)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (h >= 180 && h < 240)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (h >= 240 && h < 300)
            {
                r = x;
                g = 0;
                b = c;
            }
            else if (h >= 300 && h < 360)
            {
                r = c;
                g = 0;
                b = x;
            }

            rgb.R = (int)Math.Round((r + m) * 255);
            rgb.G = (int)Math.Round((g + m) * 255);
            rgb.B = (int)Math.Round((b + m) * 255);
            rgb.Code = $"rgb({rgb.R}, {rgb.G}, {rgb.B})";
            return rgb;
        }
    }
}