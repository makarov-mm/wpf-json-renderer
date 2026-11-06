namespace WpfJsonRenderer.Infrastructure;

/// <summary>
/// One element of the JSON scene array. Which properties are required depends on <see cref="Type"/>;
/// points are written as "X; Y" and colors as "A; R; G; B".
/// </summary>
internal sealed record JsonPrimitiveDto(
    string? Type,
    string? A,
    string? B,
    string? C,
    string? Center,
    double? Radius,
    bool? Filled,
    string? Color);
