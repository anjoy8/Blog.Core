using System.Threading.Tasks;

namespace Blog.Core.Extensions.Authorizations.Behaviors
{
    public interface IUserBehaviorService
    {

        Task<bool> CreateOrUpdateUserAccessByUid();

        Task<bool> RemoveAllUserAccessByUid();

        Task<bool> CheckUserIsNormal();

        Task<bool> CheckTokenIsNormal();
    }
}
