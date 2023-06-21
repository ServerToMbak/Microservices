using System.Threading.Tasks;
using PlatformService.Dtos;

namespace PlatformService.SyncDatServices.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDto plat);
    }
}