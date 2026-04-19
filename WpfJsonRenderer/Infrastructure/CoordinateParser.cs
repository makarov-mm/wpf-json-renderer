using System;
using System.Globalization;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Infrastructure
{
    public static class CoordinateParser
    {
        private static readonly CultureInfo CoordinateCulture = new CultureInfo("de-DE");

        public static Point2D ParsePoint(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException("Point value is empty.");
            }

            string[] parts = value.Split(';');

            if (parts.Length != 2)
            {
                throw new FormatException("Point must contain exactly two coordinates separated by ';'.");
            }

            double x = ParseDouble(parts[0]);
            double y = ParseDouble(parts[1]);

            return new Point2D(x, y);
        }

        private static double ParseDouble(string value)
        {
            return double.Parse(value.Trim(), CoordinateCulture);
        }
    }
}
