using Logic.Services;

namespace Logic.Managers
{
    public class InfoManager
    {
        private readonly VkApiService _vkApiService;

        public InfoManager(VkApiService vkApiService)
        {
            _vkApiService = vkApiService;
        }
        
        
    }
}