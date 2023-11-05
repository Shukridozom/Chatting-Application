using ChattingApplication.Core.Domains;

namespace ChattingApplication.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        int? GetUserId(string username);
    }
}
