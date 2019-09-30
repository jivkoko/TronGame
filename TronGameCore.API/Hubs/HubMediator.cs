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
    public class HubMediator
    {
        private IHubContext<TronHub> hubContext;

        public HubMediator(IHubContext<TronHub> hubContext)
        {
            this.hubContext = hubContext;

        }

        public void KeyPressed(KeyPressedRequest request)
        {
            TronGameService.KeyPressed(request.GameId, request.KeyPressed);
        }

        public Task NotifyGameOver(GameOverResponse response)
        {

            return ((DynamicHub)hubContext).Clients.Group(response.GameId).OnGameOver(response);
        }

        public Task NotifyTurnMoved(TurnMovedResponse response)
        {

            return ((DynamicHub)hubContext).Clients.Group(response.GameId).OnTurnMoved(response);
        }
    }
}
