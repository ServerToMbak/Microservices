using AutoMapper;
using CommandsService.Data;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController:ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly  IMapper _mapper;

        public CommandsController(ICommandRepo repository,IMapper mapper)
        {
            _repository =repository;
            _mapper = mapper;
        }
    }

}