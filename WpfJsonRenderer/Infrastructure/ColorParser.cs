using System;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Infrastructure
{
    public static class ColorParser
    {
        public static ArgbColor Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new FormatException("Color value is empty.");
            }

            string[] parts = value.Split(';');

            if (parts.Length != 4)
            {
                throw new FormatException("Color must contain exactly four components: A;R;G;B.");
            }

            byte a = ParseByte(parts[0]);
            byte r = ParseByte(parts[1]);
            byte g = ParseByte(parts[2]);
            byte b = ParseByte(parts[3]);

            return new ArgbColor(a, r, g, b);
        }

        private static byte ParseByte(string value)
        {
            return byte.Parse(value.Trim());
        }
    }
}
