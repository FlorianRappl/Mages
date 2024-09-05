namespace Mages.Core.Tokens;

/// <summary>
/// Helper methods for number arithmetics. 
/// </summary>
internal static class NumberHelper
{
    /// <summary>
    /// Multiplies two unsigned numbers if their product does not exceed
    /// <see cref="ulong.MaxValue"/>.
    /// </summary>
    /// <returns>A value indicating whether the numbers were multiplied.</returns>
    public static bool TryMultiply(ulong x, ulong y, out ulong product)
    {
        if (x == 0 || y == 0)
        {
            product = 0;
            return true;
        }

        // Check for overflow
        if (x > ulong.MaxValue / y)
        {
            product = 0;
            return false;
        }

        product = x * y;
        return true;
    }

    /// <summary>
    /// Adds two unsigned numbers if their sum does not exceed
    /// <see cref="ulong.MaxValue"/>.
    /// </summary>
    /// <returns>A value indicating whether the numbers were added.</returns>
    public static bool TryAdd(ulong x, ulong y, out ulong sum)
    {
        // Check for overflow
        if (x > ulong.MaxValue - y)
        {
            sum = 0;
            return false;
        }

        sum = x + y;
        return true;
    }
}