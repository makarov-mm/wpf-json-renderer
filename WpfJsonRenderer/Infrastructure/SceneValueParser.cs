using System.Globalization;
using System.Numerics;
using WpfJsonRenderer.Domain;

namespace WpfJsonRenderer.Infrastructure;

/// <summary>
/// Parses the compact values used in scene files: points "X; Y" and colors "A; R; G; B".
/// Components are separated by ';'. Coordinates accept ',' or '.' as the decimal separator
/// ("12,5" and "12.5" are both 12.5); thousands separators are rejected rather than guessed.
/// </summary>
internal static class SceneValueParser
{
    private const char ComponentSeparator = ';';

    private static readonly NumberFormatInfo DecimalCommaFormat = new() { NumberDecimalSeparator = "," };

    public static bool TryParsePoint(ReadOnlySpan<char> text, out Point2D point)
    {
        Span<double> xy = stackalloc double[2];

        if (TryParseComponents(text, xy, NumberStyles.Float) && double.IsFinite(xy[0]) && double.IsFinite(xy[1]))
        {
            point = new Point2D(xy[0], xy[1]);
            return true;
        }

        point = default;
        return false;
    }

    public static bool TryParseColor(ReadOnlySpan<char> text, out ArgbColor color)
    {
        Span<byte> argb = stackalloc byte[4];

        if (TryParseComponents(text, argb, NumberStyles.Integer))
        {
            color = new ArgbColor(argb[0], argb[1], argb[2], argb[3]);
            return true;
        }

        color = default;
        return false;
    }

    /// <summary>Parses exactly <c>values.Length</c> separated numbers, without allocations.</summary>
    private static bool TryParseComponents<T>(ReadOnlySpan<char> text, Span<T> values, NumberStyles style)
        where T : struct, INumberBase<T>
    {
        // One spare slot: if the text fills it, there are too many components.
        Span<Range> ranges = stackalloc Range[values.Length + 1];

        if (text.Split(ranges, ComponentSeparator) != values.Length)
        {
            return false;
        }

        for (int i = 0; i < values.Length; i++)
        {
            ReadOnlySpan<char> component = text[ranges[i]];
            NumberFormatInfo format = component.Contains(',') ? DecimalCommaFormat : NumberFormatInfo.InvariantInfo;

            if (!T.TryParse(component, style, format, out values[i]))
            {
                return false;
            }
        }

        return true;
    }
}
