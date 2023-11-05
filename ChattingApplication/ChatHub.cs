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
        private static Dictionary<int, int> _connectedUsers = new Dictionary<int, int>();
        public ChatHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
            SaveMessage(Convert.ToInt32(Context.UserIdentifier), user, message);
        }

        public bool IsConnected(int userId)
        {
            return _connectedUsers.ContainsKey(userId);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Convert.ToInt32(Context.UserIdentifier);

            if(_connectedUsers.ContainsKey(userId))
                _connectedUsers[userId]++;
            else
            {
                _connectedUsers.Add(userId, 1);
                await Clients.Others.SendAsync("UserConnected", userId);
            }

            base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Convert.ToInt32(Context.UserIdentifier);

            if (_connectedUsers.ContainsKey(userId))
            {
                _connectedUsers[userId]--;
                if (_connectedUsers[userId] == 0)
                {
                    _connectedUsers.Remove(userId);
                    await Clients.Others.SendAsync("UserDisconnected", userId);
                }
            }
            base.OnDisconnectedAsync(exception);
        }

        private void SaveMessage(int senderId, string receiverUsername, string body)
        {
            var receiverId = _unitOfWork.Users.GetUserId(receiverUsername);
            if (receiverId == null)
                return;

            var message = new Message()
            {
                SenderId = senderId,
                ReceiverId = receiverId.Value,
                Date = DateTime.Now,
                Body = body
            };

            _unitOfWork.Messages.Add(message);
            _unitOfWork.Complete();

        }
    }
}
