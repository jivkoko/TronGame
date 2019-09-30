using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TronGameCore.API.Games;
using TronGameCore.API.Models.Requests;
using TronGameCore.API.Hubs;
using TronGameCore.API.Services;

namespace TronGameCore.API.Controllers.API
{
    [Route("api/trongame")]
    [ApiController]
    public class TronGameController : ControllerBase
    {
        private readonly IQueue queue;
        private readonly IHubContext<TronHub> hubContext;

        public TronGameController(IQueue queue, IHubContext<TronHub> hubContext)
        {
            this.queue = queue;
            this.hubContext = hubContext;
        }

        /// <summary>
        /// Starts new Tron Game
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("start")]
        public void Start([FromBody]StartGameRequest model)
        {
            if (model != null && ModelState.IsValid)
            {
                try
                {
                    //Use Start REST endpoint because Coravel lib queued jobs will execute every 30 seconds
                    queue.QueueAsyncTask(() => StartGameBackground(new HubMediator(hubContext), model.GameId, model.BorderWidth, model.BorderHeight));
                }
                catch (Exception)
                {
                    //Log
                    throw;
                }
            }
        }

        private async Task StartGameBackground(HubMediator mediator, string gameId, ushort borderWidth, ushort borderHeight)
        {
            TronGameService.StartGame(mediator, gameId, borderWidth, borderHeight);
        }

        [HttpPut]
        public async Task KeyPressed(KeyPressedRequest model)
        {
            TronGameService.KeyPressed(model.GameId, model.KeyPressed);
        }


        /// <summary>
        /// Aborts existing Tron Game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task Abort(string gameId)
        //{
        //    if (!string.IsNullOrWhiteSpace(gameId) && ModelState.IsValid)
        //    {

        //        try
        //        {
        //            TronGameService.AbortGame(gameId);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
           
        //}








        //[HttpPost]
        //public async Task Start()
        //{
        //    string jobId = Guid.NewGuid().ToString("N");

        //    queue.QueueAsyncTask(() => PerformBackgroundJob(jobId));


        //}

    }
}