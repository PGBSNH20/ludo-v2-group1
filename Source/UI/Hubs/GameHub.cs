using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Ludo.Web.Hubs
{
    public class GameHub : Hub
    {
        private readonly ILudoDataAccess _ludoData;
        public GameHub(ILudoDataAccess ludoData)
        {
            _ludoData = ludoData;
        }
        public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
        public async Task SendMessage(string group, string user, string message)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
        }

        public async Task PlayerTurn(string group)
        {
            string playerName = await _ludoData.GetPlayerTurn(group);
            await Clients.Group(group).SendAsync("GetPlayerTurn", playerName);
        }
        public async Task AddPlayerTurn(string group, string name)
        {
            await _ludoData.AddPlayerTurn(group, name);
            await Clients.Group(group).SendAsync("GetPlayerTurn", name);
        }
    }
}
