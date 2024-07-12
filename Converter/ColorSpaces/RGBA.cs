namespace ColorUtil.Converter.ColorSpaces
{
    public class RGBA
    {
        public string Name { get; set; } = "RGBA";
        public int? R { get; set; }
        public int? G { get; set; }
        public int? B { get; set; }
        public double? A { get; set; }
        public string? Code { get; set; }
        public const string Pattern = @"^\s*rgba\s*\(\s*\d{1,3}\s*,\s*\d{1,3}\s*,\s*\d{1,3}\s*,\s*(0|0?\.\d+|1(\.0)?)\s*\)\s*$";

        // Convert RGB to RGBA
        public RGBA From(RGB rgb)
        {
            RGBA rgba = new()
            {
                R = rgb.R,
                G = rgb.G,
                B = rgb.B,
                A = 1.0
            };

            rgba.Code = $"rgba({rgba.R}, {rgba.G}, {rgba.B}, {rgba.A})";
            return rgba;
        }

        // Convert RGBA to RGB
        public RGB To(string color)
        {
            // Convert an RGBA string in this format: rgba(0, 0, 0, 0) to the class RGB
            // Remove all spaces from the string.
            color.ToLower();
            color = color.Replace(" ", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("rgba", "");

            // Split the string into an array.
            string[] rgba = color.Split(',');
            int r = int.Parse(rgba[0]);
            int g = int.Parse(rgba[1]);
            int b = int.Parse(rgba[2]);
            double a = double.Parse(rgba[3]);

            RGB rgb = new()
            {
                R = r,
                G = g,
                B = b,
                Code = $"rgb({r}, {g}, {b})"
            };

            return rgb;
        }
    }
}