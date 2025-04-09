using AzerIsiq.Extensions.Enum;

namespace AzerIsiq.Services.Helpers;

public static class SubscriberStatusHelper
{
    public static int AdvanceStatus(int currentStatus, SubscriberStatus expectedStatus)
    {
        if (currentStatus == (int)expectedStatus)
        {
            return currentStatus + 1;
        }
        return currentStatus;
    }

    public static bool IsStatus(int currentStatus, SubscriberStatus status)
    {
        return currentStatus == (int)status;
    }
}
