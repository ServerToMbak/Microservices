using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        //platforms
        IEnumerable<Platform> GetAllPlatforms();
        void createPlatform(Platform plat);
        bool PlatformExists(int platformId);       
        bool ExternalPlatformExists(int ExternalPlatformId);
        //commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int CommandId);
        void CreateCommand(int platformId,Command command);
    
    }
}