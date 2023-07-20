using PlatformService.Dtos;

namespace PlatformService.AsyncDatServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDtodto);
    }
}