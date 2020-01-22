using System.Threading.Tasks;
using Logic.Dto;
using Logic.Managers;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class InfoController : Controller
    {
        private readonly VkApiService _vkApiService;

        public InfoController(VkApiService vkApiService)
        {
            _vkApiService = vkApiService;
        }

        
        [HttpPost("wall")]
        public JsonResult Wall(long id) => new JsonResult(_vkApiService.GetWallMessage(id));
    }
}