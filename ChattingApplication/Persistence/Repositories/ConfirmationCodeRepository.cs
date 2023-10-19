using ChattingApplication.Core.Domains;
using ChattingApplication.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChattingApplication.Persistence.Repositories
{
    public class ConfirmationCodeRepository : Repository<ConfirmationCode>, IConfirmationCodeRepository
    {
        public ConfirmationCodeRepository(AppDbContext context)
            :base(context)
        {

        }

    }
}
