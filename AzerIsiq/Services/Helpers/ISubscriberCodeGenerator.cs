using AzerIsiq.Models;

namespace AzerIsiq.Services.Helpers;

public interface ISubscriberCodeGenerator
{
    string Generate(Subscriber subscriber);
}