namespace FinSharkMarket.utils;

public static class DateTimeUtils
{
    public static DateTime ToUtc(DateTime dateTime)
    {
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}