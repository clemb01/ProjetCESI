using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.SignalR
{
    public class MessageHub : Hub
    {
        public Task Join(string ressourceId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, ressourceId);
        }

        public Task LeaveRoom(string ressourceId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, ressourceId);
        }

        public async Task SendMessage(string sender, string message, string ressourceId)
        {
            await Clients.Group(ressourceId).SendAsync("ReceiveMessage", sender, message).ConfigureAwait(true);
        }
    }
}
