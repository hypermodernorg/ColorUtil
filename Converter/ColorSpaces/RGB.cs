using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorUtil.Converter.ColorSpaces
{
    public class RGB
    {
        public string Name { get; set; } = " RGB";
        public int? R { get; set; }
        public int? G { get; set; }
        public int? B { get; set; }
        public string? Code { get; set; }
        public const string Pattern = @"^\s*rgb\s*\(\s*\d{1,3}\s*,\s*\d{1,3}\s*,\s*\d{1,3}\s*\)\s*$";

        public RGB To(string color)
        {
            // Remove all spaces from the string.
            color = color.Replace(" ", "");

            // Remove the rgb( and ) from the string.
            color = color.Substring(4, color.Length - 5);

            // Split the string into an array.
            string[] rgb = color.Split(',');

            // Convert the RGB color to RGB.
            RGB rgbColor = new()
            {
                R = int.Parse(rgb[0]),
                G = int.Parse(rgb[1]),
                B = int.Parse(rgb[2])
            };

            // add the rgb string to the object
            rgbColor.Code = $"rgb({rgbColor.R}, {rgbColor.G}, {rgbColor.B})";

            return rgbColor;
        }
    }
}
