using ChattingApplication.Core.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChattingApplication
{
    [Authorize(Roles = RoleName.Verified)]
    public class ChatHub : Hub
    {
        public ChatHub()
        {

        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.User(user).SendAsync("ReceiveMessage", Context.UserIdentifier, message);

        }
    }
}
