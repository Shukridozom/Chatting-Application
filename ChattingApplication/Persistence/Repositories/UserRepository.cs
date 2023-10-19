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

    }
}
