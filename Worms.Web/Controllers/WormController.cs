using System;
using Microsoft.AspNetCore.Mvc;
using Worms.Domain;
using Worms.Web.Behaviour;

namespace Worms.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WormController : ControllerBase
    {
        private readonly AbstractWormBehaviour _behaviour;

        public WormController(AbstractWormBehaviour behaviour)
        {
            _behaviour = behaviour;
        }


        [HttpPost]
        [Route("{name}/getAction")]
        public object Post([FromBody] WorldState worldState, string name, int step, int run)
        {
            var response = _behaviour.GetResponse(worldState, name, run, step);
            Console.WriteLine(response.Direction.ToString());
            if (response.Direction == null)
                return new Empty();
            return response;
        }
    }

    public class Empty
    {
    }
}