namespace ColorUtil.Converter.ColorSpaces
{
    // class for the YCbCr colorspace
    public class YCbCr
    {
        public string Name { get; set; } = "YCbCr";
        public double? Y { get; set; }
        public double? Cb { get; set; }
        public double? Cr { get; set; }
        public string? Code { get; set; }
        //public const string Pattern = @"^\s*ycbcr\s*\(\s*\d{1,3}\s*,\s*\d{1,3}\s*,\s*\d{1,3}\s*\)\s*$";
        public const string Pattern = @"^\s*ycbcr\s*\(\s*([0-9]*\.?[0-9]+)\s*,\s*([0-9]*\.?[0-9]+)\s*,\s*([0-9]*\.?[0-9]+)\s*\)\s*$";


        public YCbCr From(RGB rgb)
        {
            // Convert RGB to YCbCr
            double r = (double)(rgb.R / 255.0);
            double g = (double)(rgb.G / 255.0);
            double b = (double)(rgb.B / 255.0);

            YCbCr ycbcr = new YCbCr
            {
                Y = 0.299 * r + 0.587 * g + 0.114 * b,
                Cb = -0.168736 * r - 0.331264 * g + 0.5 * b + 0.5,
                Cr = 0.5 * r - 0.418688 * g - 0.081312 * b + 0.5
            };

            ycbcr.Code = $"ycbcr({ycbcr.Y}, {ycbcr.Cb}, {ycbcr.Cr})";

            return ycbcr;
        }

        public RGB To(string ycbcr)
        {
            // Parse the YCbCr string
            var ycbcrValues = ycbcr.Replace("ycbcr", "").Replace(")", "").Replace("(", "").Replace(" ", "").Split(',');
            double y = double.Parse(ycbcrValues[0].Trim());
            double cb = double.Parse(ycbcrValues[1].Trim()) - 0.5;
            double cr = double.Parse(ycbcrValues[2].Trim()) - 0.5;

            // Convert YCbCr to RGB
            double r = y + 1.402 * cr;
            double g = y - 0.344136 * cb - 0.714136 * cr;
            double b = y + 1.772 * cb;

            RGB rgb = new()
            {
                R = (int)Math.Max(0, Math.Min(255, r * 255)),
                G = (int)Math.Max(0, Math.Min(255, g * 255)),
                B = (int)Math.Max(0, Math.Min(255, b * 255))
            };

            rgb.Code = $"rgb({rgb.R}, {rgb.G}, {rgb.B})";

            return rgb;
        }
    }
}