using AzerIsiq.Models;

namespace AzerIsiq.Services.Helpers;

public class SubscriberCodeGenerator : ISubscriberCodeGenerator
{
    public string Generate(Subscriber subscriber)
    {
        var districtId = subscriber.DistrictId.ToString().PadLeft(2, '0');
        var territoryId = (subscriber.TerritoryId?.ToString() ?? "00").PadLeft(2, '0');
        var streetId = (subscriber.StreetId?.ToString() ?? "000").PadLeft(3, '0');
        var building = (subscriber.Building ?? "0").PadLeft(4, '0');
        var apartment = (subscriber.Apartment ?? "0").PadLeft(4, '0');

        return $"{districtId}{territoryId}{streetId}{building}{apartment}";
    }
}