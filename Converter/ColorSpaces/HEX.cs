using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorUtil.Converter.ColorSpaces
{
    public class HEX
    {
        public string Name { get; set; } = "HEX";
        public string? Code { get; set; }

        public const string Pattern = @"^([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";

        // Convert RGB to HEX
        public HEX From(RGB rgb)
        {
            HEX hex = new();
            hex.Code = $"#{rgb.R:X2}{rgb.G:X2}{rgb.B:X2}";
            return hex;
        }

        public RGB To(string color)
        {
            // Remove the # at the beginning of the string.
            color = color.Replace("#", "");

            // Check if the color is in short format.
            if (color.Length == 3)
            {
                color = $"{color[0]}{color[0]}{color[1]}{color[1]}{color[2]}{color[2]}";
            }

            // Convert the HEX color to RGB.
            RGB rgb = new()
            {
                R = int.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                G = int.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                B = int.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber)
            };

            // add the rgb string to the object

            rgb.Code = $"rgb({rgb.R}, {rgb.G}, {rgb.B})";

            return rgb;
        }
    }
}
