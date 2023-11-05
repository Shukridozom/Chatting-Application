using ChattingApplication.Core;
using ChattingApplication.Core.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChattingApplication
{
    [Authorize(Roles = RoleName.Verified)]
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
            SaveMessage(Convert.ToInt32(Context.UserIdentifier), user, message);
        }

        private void SaveMessage(int senderId, string receiverUsername, string body)
        {
            var receiver = _unitOfWork.Users.SingleOrDefault(u => u.Username == receiverUsername);
            if (receiver == null)
                return;

            var message = new Message()
            {
                SenderId = senderId,
                ReceiverId = receiver.Id,
                Date = DateTime.Now,
                Body = body
            };

            _unitOfWork.Messages.Add(message);
            _unitOfWork.Complete();

        }
    }
}
