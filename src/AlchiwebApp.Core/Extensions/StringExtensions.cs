namespace System;

/// <summary>
/// String extensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Adds a separator between two strings if both are not null or empty.
    /// </summary>
    /// <param name="separator"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static string AddSeparatorTo(this string separator, string? left, string? right)
        => left + (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right) ? "" : separator) + right;
}
