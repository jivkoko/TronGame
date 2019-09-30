using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TronGameCore.API.Models.Requests;
using TronGameCore.API.Models.Responses;
using TronGameCore.API.Services;

namespace TronGameCore.API.Hubs
{
    public class TronHub : DynamicHub //Hub<ITronHub>, ITronHub
    {
        public async Task AssociateJob(string jobId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, jobId);
        }

        public void KeyPressed(KeyPressedRequest request)
        {
            TronGameService.KeyPressed(request.GameId, request.KeyPressed);
        }

        public Task NotifyTurnMoved(TurnMovedResponse response)
        {

            return Clients.Group(response.GameId).OnTurnMoved(response);
        }

        public Task NotifyGameOver(GameOverResponse response)
        {

            return Clients.Group(response.GameId).OnGameOver(response);
        }

    }
}
