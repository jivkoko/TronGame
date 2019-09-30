using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TronGameCore.API.Games;
using TronGameCore.API.Hubs;

namespace TronGameCore.API.Services
{
    public class TronGameService
    {
        private static Dictionary<string, Tron> StartedGames = new Dictionary<string, Tron>();

        private static void AddNewGame(string gameId, Tron game)
        {
            if (!StartedGames.ContainsKey(gameId))
            {
                StartedGames.Add(gameId, game);
            }
        }

        private static Tron GetExistingGame(string gameId)
        {
            if (StartedGames.ContainsKey(gameId))
            {
                return StartedGames[gameId];
            }

            throw new Exception($"Not existing game with ID: {gameId}");
        }

        private static void RemoveGame(string gameId)
        {
            if (StartedGames.ContainsKey(gameId))
            {
                StartedGames.Remove(gameId);
            }
        }

        internal static void KeyPressed(string gameId, Keys keyPressed)
        {
            var game = TronGameService.GetExistingGame(gameId);

            game.ChangePlayerDirection(keyPressed);
        }

        internal static void StartGame(HubMediator mediator, string gameId, ushort borderWidth, ushort borderHeight)
        {
            var tronGame = new Tron(mediator, gameId, borderWidth, borderHeight);
            tronGame.GameOver += GameOver;
            tronGame.TurnMoved += TurnMoved;

            TronGameService.AddNewGame(gameId, tronGame);

            tronGame.Play();
        }

        internal static void AbortGame(string gameId)
        {
            var game = TronGameService.GetExistingGame(gameId);

            //TODO: Abort game 
            //game.Abort()

            TronGameService.RemoveGame(gameId);

        }

        internal static void GameOver(object sender, GameOverEventArgs e)
        {
            //TODO: notify Hub and specific Client
        }

        internal static void TurnMoved(object sender, TurnMovedEventArgs e)
        {
            //TODO: notify Hub and specific Client
        }
    }
}
