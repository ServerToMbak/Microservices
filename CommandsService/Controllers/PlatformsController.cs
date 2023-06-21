using System;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{ 
    [Route("api/c/[Controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {

        public PlatformsController()
        {

        }

        [HttpPost]
        public ActionResult TextInBoundConnection()
        {
            Console.WriteLine("--> Inbound POST # CommandService");

            return Ok("Inbound test of from platforms Controller");
        }

    }
}                                                                                                                                 