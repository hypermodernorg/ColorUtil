namespace ColorUtil.Converter.ColorSpaces
{
    public class HSV
    {
        public string Name { get; set; } = "HSV";
        public int? H { get; set; }
        public int? S { get; set; }
        public int? V { get; set; }
        public string? Code { get; set; }
        public const string Pattern = @"^\s*hsv\s*\(\s*\d{1,3}\s*,\s*\d{1,3}%\s*,\s*\d{1,3}%\s*\)\s*$";



        // Convert RGB to HSV
        public HSV From(RGB rgb)
        {
            double r = (double)(rgb.R / 255.0);
            double g = (double)(rgb.G / 255.0);
            double b = (double)(rgb.B / 255.0);
            double cmax = Math.Max(r, Math.Max(g, b));
            double cmin = Math.Min(r, Math.Min(g, b));
            double delta = cmax - cmin;
            double h = 0;
            double s = 0;
            double v = cmax;

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

                if (cmax != 0)
                {
                    s = delta / cmax;
                }
            }

            HSV hsv = new()
            {
                H = (int)Math.Round(h),
                S = (int)Math.Round(s * 100),
                V = (int)Math.Round(v * 100)
            };

            hsv.Code = $"hsv({hsv.H}, {hsv.S}%, {hsv.V}%)";
            return hsv;
        }

        // Convert HSV to RGB
        public RGB To(string color)
        {
            // Convert an HSV string in this format: hsv(0, 0%, 0%) to the class RGB
            // Remove all spaces from the string.
            color = color.Replace(" ", "");

            //remove hsv regardless of case
            color = color.ToLower().Replace("hsv", "");

            //remove the ( and ) from the string
            color = color.Replace("(", "");
            color = color.Replace(")", "");

            // Split the string into an array.
            string[] hsv = color.Split(',');
            int h = int.Parse(hsv[0]);
            int s = int.Parse(hsv[1].Replace("%", ""));
            int v = int.Parse(hsv[2].Replace("%", ""));
            RGB rgb = new()
            {
                R = 0,
                G = 0,
                B = 0
            };

            // Convert the HSV color to RGB.
            double s_norm = s / 100.0;
            double v_norm = v / 100.0;
            double c = v_norm * s_norm;
            double x = c * (1 - Math.Abs(h / 60.0 % 2 - 1));
            double m = v_norm - c;
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