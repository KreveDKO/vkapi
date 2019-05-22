using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private GraphManager _graphManager;
        public GraphController(GraphManager graphManager)
        {
            _graphManager = graphManager;
        }

        [HttpGet("getfriends")]
        public IActionResult GetFriends(long userId)
        {
            var links = _graphManager.GetFriends(userId);
            var nodes = _graphManager.GetUserNodes(links);
            return new JsonResult(new { nodes, links });
        }
        [HttpGet("getgraph")]
        public IActionResult GetGraph(long userId = 0)
        {
            var links = _graphManager.GetUserLinks(userId);
            var nodes = _graphManager.GetUserNodes(links);
            return new JsonResult(new { nodes, links });
        }
    }
}
