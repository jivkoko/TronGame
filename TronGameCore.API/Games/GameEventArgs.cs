using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TronGameCore.API.Games
{
    public class GameEventArgs : EventArgs
    {
        public string  GameId { get; set; }
    }

    public class GameOverEventArgs : GameEventArgs
    {
        public GameResult Result { get; set; }
        public ushort FirstPlayerScore { get; set; }
        public ushort SecondPlayerScore { get; set; }
    }

    public class TurnMovedEventArgs : GameEventArgs
    {
        public int FirstPlayerRow { get; set; }
        public int FirstPlayerColumn { get; set; }

        public int SecondPlayerRow { get; set; }
        public int SecondPlayerColumn { get; set; }
    }
}
