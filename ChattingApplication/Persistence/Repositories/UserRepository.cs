using ChattingApplication.Core.Domains;
using ChattingApplication.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChattingApplication.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context)
            :base(context)
        {

        }

        public int? GetUserId(string username)
        {
            return Context.Users.SingleOrDefault(u => u.Username == username)?.Id;
        }
    }
}
