using Microsoft.AspNetCore.SignalR;
using SignalRMessage.Data;
using SignalRMessage.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRMessage.Hubs
{
    public class ChatHub : Hub
    {
        public async Task GetNickName(string nickName)
        {
            Client client = new Client
            {
                ConnectionId = Context.ConnectionId,
                NickName = nickName
            };

            ClientSource.Clients.Add(client);
            await Clients.Others.SendAsync("clientJoined", nickName);

            await Clients.All.SendAsync("clients", ClientSource.Clients);
        }


        // Send Message Method
        public async Task SendMessageAsync(string message, string clientName)
        {
            clientName = clientName.Trim();
            Client sentClient = ClientSource.Clients.FirstOrDefault(k => k.ConnectionId == Context.ConnectionId);
            if (clientName == "Tümü")
            {
                await Clients.Others.SendAsync("receiverMessage", message, sentClient.NickName);
            }
            else
            {
                Client client = ClientSource.Clients.FirstOrDefault(k => k.NickName == clientName);
                await Clients.Client(client.ConnectionId).SendAsync("receiverMessage", message, sentClient.NickName);
            }
        }


        // Add Groups
        public async Task AddGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            // Grup Adını Buraya Ekle GroupSource
            Group group = new Group { GroupName = groupName };
            group.Clients.Add(ClientSource.Clients.FirstOrDefault(k => k.ConnectionId == Context.ConnectionId));
            GroupSource.Groups.Add(group);

            // Ve Sonra (Group Oluşturuldugunda) Tüm Kullanıcılara Bildirim Gönder
            await Clients.All.SendAsync("groups", GroupSource.Groups);
        }


        // Clientların Odalara Gruplara Girmesi
        public async Task AddClientToGroups(IEnumerable<string> groupNames)
        {
            Client client = ClientSource.Clients.FirstOrDefault(f => f.ConnectionId == Context.ConnectionId);
            foreach (var group in groupNames)
            {
                Group _group = GroupSource.Groups.FirstOrDefault(w => w.GroupName == group);

                //Kontrolu Sagla 1 Client 1 Kere Gruba Girebilir
                var result = _group.Clients.Any(k => k.ConnectionId == Context.ConnectionId);
                if (!result)
                {
                    _group.Clients.Add(client);
                    await Groups.AddToGroupAsync(Context.ConnectionId, group);
                }
            }
        }


        // Groputa Ne Kadar Client varsa getir (Functiondan Dönen Değer)
        public async Task GetClientToGroup(string groupName)
        {
            Group group = GroupSource.Groups.FirstOrDefault(g => g.GroupName == groupName);
            await Clients.Caller.SendAsync("clients", groupName == "-1" ? ClientSource.Clients : group.Clients);
        }

        // Seçili Grup Altındaki Tüm Clientlere Mesaj Gönderme!!!
        public async Task SendMessageToGroupAsync(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("receiverMessage", message, ClientSource.Clients
                .FirstOrDefault(f => f.ConnectionId == Context.ConnectionId).NickName);
        }
    }
}
