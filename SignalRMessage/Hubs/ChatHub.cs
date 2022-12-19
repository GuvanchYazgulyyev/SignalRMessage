using Microsoft.AspNetCore.SignalR;
using SignalRMessage.Data;
using SignalRMessage.Models;
using System.Threading.Tasks;

namespace SignalRMessage.Hubs
{
    public class ChatHub : Hub
    {
        public async Task GetNockName(string nickName)
        {
            Client client = new Client
            {
                ConnectionId = Context.ConnectionId,
                NickName = nickName
            };
            ClientSource.Clients.Add(client);
           await Clients.Others.SendAsync("clientJoined", nickName);
        }
    }
}
