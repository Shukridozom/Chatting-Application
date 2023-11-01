using ChattingApplication.Core.Repositories;

namespace ChattingApplication.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IConfirmationCodeRepository ConfirmationCodes { get; }
        IMessageRepository Messages { get; }
        int Complete();
    }
}
