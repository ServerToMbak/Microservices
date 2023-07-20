using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory,
        IMapper mapper)
        {
            _mapper =mapper;
            _scopeFactory=scopeFactory;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    // To Do
                    break;

                default:
                    break;
            }
        }
        
        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch(eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Publish Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determined event type");
                    return EventType.Undetermined;

            }
        }
        private void addPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>(); // Create Scope for command repository from Data folder

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if(!repo.ExternalPlatformExists(plat.ExternalId))
                    {
                        repo.createPlatform(plat);
                        repo.SaveChanges();
                    }else
                    {
                        Console.WriteLine("--> Platform Already Exists.....");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"-> Could not add Platform To the DB {ex.Message}");
                }
            }
        }
        
    }
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}