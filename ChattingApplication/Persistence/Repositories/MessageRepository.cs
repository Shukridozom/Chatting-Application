using ChattingApplication.Core.Domains;
using ChattingApplication.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChattingApplication.Persistence.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(AppDbContext context)
            :base(context)
        {

        }

    }
}
