using System.Threading.Tasks;
using Core.DataContext;
using Core.Dto;

namespace Logic.Interfaces.Managers
{
    public interface IUpdateManager
    {
        Task UpdateFriendList(FriendsUpdateDto dto, ApplicationContext context = null);
        
        Task UpdateFriendsGroupsList(long userId);

        Task UpdateGroupsList(long userId, ApplicationContext context = null);

        long UpdateUserInfo(long userId, ApplicationContext context = null);

        long UpdateGroupInfo(GroupUpdateDto dto, ApplicationContext context = null);

        Task UpdateMessages();

        Task UpdateFriendsAudio();
        
        Task UpdateAudio(long id, ApplicationContext context = null);

        Task UpdateVideo(long id, ApplicationContext context = null);

        Task UpdateWall(WallUpdateDto dto);
    }
}