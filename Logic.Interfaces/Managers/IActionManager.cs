using System.Threading.Tasks;
using Core.Dto;

namespace Logic.Interfaces.Managers
{
    public interface IActionManager
    {
        Task SendWallMessages(SendWallMessageDto dto);
    }
}