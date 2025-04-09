using AzerIsiq.Extensions.Enum;

namespace AzerIsiq.Services.Helpers;

public static class SubscriberStatusHelper
{
    public static void AdvanceStatus(ref int currentStatus, SubscriberStatus expectedStatus)
    {
        if (currentStatus == (int)expectedStatus)
        {
            currentStatus++;
        }
    }

    public static bool IsStatus(ref int currentStatus, SubscriberStatus status)
    {
        return currentStatus == (int)status;
    }
}