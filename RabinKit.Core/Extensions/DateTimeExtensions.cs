namespace RabinKit.Core.Extensions;

public static class DateTimeExtensions
{
    public static string ToLocalString(this DateTime dateTime)
        => dateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
}