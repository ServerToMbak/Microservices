using System;
using System.Collections.Generic;
using System.Linq;
using CommandsService.Models;

namespace CommandsService.Data
{

    public class CommandRepo : ICommandRepo
    {
        private AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if(command == null)
            {
                throw new ArgumentNullException();
            }

            command.PlatformId = platformId;
            _context.Commands.Add(command);
        
        }

        public void createPlatform(Platform plat)
        {
            if(plat==null)
            {
                throw new ArgumentNullException();
            } 
            
            _context.Platforms.Add(plat);
        }

        public bool ExternalPlatformExists(int ExternalPlatformId)
        {
            return _context.Platforms.Any(X => X.ExternalId == ExternalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands
                    .Where(c=> c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                .Where(c=>c.PlatformId == platformId)
                .OrderBy(c=>c.Platform.Name);   
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p=> p.Id ==platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges()>=0);
        }
    }
}