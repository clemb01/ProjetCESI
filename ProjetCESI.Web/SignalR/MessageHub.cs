using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.SignalR
{
    public class MessageHub : Hub
    {
        public async Task Join(string ressourceId, string sender)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ressourceId);

            sender = sender.Contains("Anonyme") ? "Anonyme" : sender;
            await Clients.OthersInGroup(ressourceId).SendAsync("JoinedRoom", sender, $"{sender} à rejoint la salle").ConfigureAwait(true);
        }

        public async Task LeaveRoom(string ressourceId, string sender)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ressourceId);

            sender = sender.Contains("Anonyme") ? "Anonyme" : sender;
            await Clients.OthersInGroup(ressourceId).SendAsync("JoinedRoom", sender, $"{sender} à quitté la salle").ConfigureAwait(true);
        }

        public async Task SendMessage(string sender, string message, string ressourceId)
        {
            await Clients.Group(ressourceId).SendAsync("ReceiveMessage", sender, message).ConfigureAwait(true);
        }
    }
}
