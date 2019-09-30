using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using TronGameCore.API.Hubs;

namespace TronGameCore.API.Games
{
    public class Tron
    {
        private const byte MoveInterval = 100;

        private const byte Left = 0;
        private const byte Right = 1;
        private const byte Up = 2;
        private const byte Down = 3;

        public readonly string GameId;

        private HubMediator mediator;

        private ushort firstPlayerScore = 0;
        private ushort secondPlayerScore = 0;

        private byte firstPlayerDirection = Right;
   
        private int firstPlayerRow; //start row
        private int firstPlayerColumn; //start column

        private byte secondPlayerDirection = Left;
    
        private int secondPlayerRow; //start row
        private int secondPlayerColumn; //start column

        private bool[,] isCellUsed;

        private ushort boardWidth;
        private ushort boardHeight;

        public event EventHandler<GameOverEventArgs> GameOver;
        public event EventHandler<TurnMovedEventArgs> TurnMoved;

        public Tron(HubMediator mediator, string gameId, ushort boardWidth, ushort boardHeight)
        {
            this.mediator = mediator;

            this.GameId = gameId;

            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;

            InitalizeGameBoard();        
        }

        private void InitalizeGameBoard()
        {
            isCellUsed = new bool[boardWidth, boardHeight];

            //initalize players position - horizontal middle
            firstPlayerRow = 0;
            firstPlayerColumn = boardWidth / 2;

            secondPlayerRow = boardHeight - 1;
            secondPlayerColumn = boardWidth / 2;
        }

        public void Play()
        {
            while (true)
            {
                MovePlayers();

                bool firstPlayerLoses = PlayerLose(firstPlayerRow, firstPlayerColumn);
                bool secondPlayerLoses = PlayerLose(secondPlayerRow, secondPlayerColumn);

                if (firstPlayerLoses && secondPlayerLoses)
                {
                    firstPlayerScore++;
                    secondPlayerScore++;
  
                    RiseGameOverEvent(GameId, GameResult.DrawGame, firstPlayerScore, secondPlayerScore);

                    ResetGame();
                }
                if (firstPlayerLoses)
                {
                    secondPlayerScore++;

                    RiseGameOverEvent(GameId, GameResult.FirstPlayerWin, firstPlayerScore, secondPlayerScore);

                    ResetGame();
                }
                if (secondPlayerLoses)
                {
                    firstPlayerScore++;

                    RiseGameOverEvent(GameId, GameResult.SecondPlayerWin, firstPlayerScore, secondPlayerScore);

                    ResetGame();
                }

                isCellUsed[firstPlayerColumn, firstPlayerRow] = true;
                isCellUsed[secondPlayerColumn, secondPlayerRow] = true;


                RiseTurnMovedEvent(GameId, firstPlayerRow, firstPlayerColumn, secondPlayerRow, secondPlayerColumn);

                Thread.Sleep(MoveInterval);
            }
        }

        private void RiseGameOverEvent(string gameId, GameResult result, ushort firstPlayerScore, ushort secondPlayerScore)
        {
            GameOverEventArgs args = new GameOverEventArgs();
            args.GameId = gameId;
            args.Result = result;
            args.FirstPlayerScore = firstPlayerScore;
            args.SecondPlayerScore = secondPlayerScore;
            OnGameOver(args);

            mediator.NotifyGameOver(new Models.Responses.GameOverResponse() {
                GameId = gameId,
                Result = result,
                FirstPlayerScore = firstPlayerScore,
                SecondPlayerScore = secondPlayerScore
            });
        }

        private void RiseTurnMovedEvent(string gameId, int firstPlayerRow, int firstPlayerColumn, int secondPlayerRow, int secondPlayerColumn)
        {
            TurnMovedEventArgs args = new TurnMovedEventArgs();
            args.GameId = gameId;
            args.FirstPlayerRow = firstPlayerRow;
            args.FirstPlayerColumn = firstPlayerColumn;
            args.SecondPlayerRow = secondPlayerRow;
            args.SecondPlayerColumn = secondPlayerColumn;
            OnTurnMoved(args);

            mediator.NotifyTurnMoved(new Models.Responses.TurnMovedResponse() { 
                GameId = gameId,
                FirstPlayerRow = firstPlayerRow,
                FirstPlayerColumn = firstPlayerColumn,
                SecondPlayerRow = secondPlayerRow,
                SecondPlayerColumn = secondPlayerColumn
            });
        }

        private void OnGameOver(GameOverEventArgs e)
        {
            EventHandler<GameOverEventArgs> handler = GameOver;
            handler?.Invoke(this, e);
        }

        private void OnTurnMoved(TurnMovedEventArgs e)
        {
            EventHandler<TurnMovedEventArgs> handler = TurnMoved;
            handler?.Invoke(this, e);
        }

        //TODO: Reset or Abort
        private void ResetGame()
        {
            isCellUsed = new bool[boardWidth, boardHeight];

            InitalizeGameBoard();

            firstPlayerDirection = Right;
            secondPlayerDirection = Left;

            MovePlayers();
        }

        public void MovePlayers()
        {
            if (firstPlayerDirection == Right)
            {
                firstPlayerColumn++;
            }
            if (firstPlayerDirection == Left)
            {
                firstPlayerColumn--;
            }
            if (firstPlayerDirection == Up)
            {
                firstPlayerRow--;
            }
            if (firstPlayerDirection == Down)
            {
                firstPlayerRow++;
            }


            if (secondPlayerDirection == Right)
            {
                secondPlayerColumn++;
            }
            if (secondPlayerDirection == Left)
            {
                secondPlayerColumn--;
            }
            if (secondPlayerDirection == Up)
            {
                secondPlayerRow--;
            }
            if (secondPlayerDirection == Down)
            {
                secondPlayerRow++;
            }
        }

        public void ChangePlayerDirection(Keys keyPressed)
        {
            //first player
            if (keyPressed == Keys.A && firstPlayerDirection != Right)
                firstPlayerDirection = Left;

            if (keyPressed == Keys.D && firstPlayerDirection != Left)
                firstPlayerDirection = Right;


            if (keyPressed == Keys.W && firstPlayerDirection != Down)
                firstPlayerDirection = Up;

            if (keyPressed == Keys.S && firstPlayerDirection != Up)
                firstPlayerDirection = Down;

            //second player
            if (keyPressed == Keys.Left && secondPlayerDirection != Right)
                secondPlayerDirection = Left;

            if (keyPressed == Keys.Right && secondPlayerDirection != Left)
                secondPlayerDirection = Right;

            if (keyPressed == Keys.Up && secondPlayerDirection != Down)
                secondPlayerDirection = Up;

            if (keyPressed == Keys.Down && secondPlayerDirection != Up)
                secondPlayerDirection = Down;
        }

        public bool PlayerLose(int row, int col)
        {
            //board dorders
            if (row < 0)
                return true;

            if (col < 0)
                return true;

            if (row >= boardHeight)
                return true;

            if (col >= boardWidth)
                return true;

            //already used cell
            if (isCellUsed[col, row])
                return true;


            return false;
        }
    }
}