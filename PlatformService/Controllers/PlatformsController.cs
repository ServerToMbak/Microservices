using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDatServices.Http;

namespace PlatformService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController :ControllerBase
    {
        private  readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private ICommandDataClient _commandDataClient;

        public PlatformsController(
            IPlatformRepo repository, 
            IMapper mapper,
            ICommandDataClient commandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms...");

            var platformItem = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItem));
        } 
        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);
            if(platformItem != null ) 
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }
            return NotFound();  
        }   

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);//Mapped Platform to PlatformCreateDto
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges(); 

            var PlatformReadDto = _mapper.Map<PlatformReadDto>(platformModel);//mapped Platform to platfornReadDto for returning the data which created
            
            try
            {
                await _commandDataClient.SendPlatformToCommand(PlatformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send Synchronously: {ex.Message}");  
            }

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = PlatformReadDto.Id}, PlatformReadDto);
        }


    }
}