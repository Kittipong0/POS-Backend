namespace POS.Common.Helpers;

public static class Utilities
{
    public static string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}
