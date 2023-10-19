using ChattingApplication.Core;
using ChattingApplication.Core.Domains;
using ChattingApplication.Core.Repositories;
using ChattingApplication.Persistence.Repositories;

namespace ChattingApplication.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; }
        public IConfirmationCodeRepository ConfirmationCodes { get; }
        //public UnitOfWork(AppDbContext context)
        //{
        //    _context = context;
        //    Users = new UserRepository(context);
        //    ConfirmationCodes = new ConfirmationCodeRepository(context);
        //}

        public UnitOfWork(AppDbContext context,
            IUserRepository userRepository,
            IConfirmationCodeRepository confirmationCodeRepository)
        {
            _context = context;
            Users = userRepository;
            ConfirmationCodes = confirmationCodeRepository;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
