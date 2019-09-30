using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TronGameCore.API.Games;

namespace TronGameCore.API.Models.Responses
{
    public class GameOverResponse : GameResponse
    {
        public GameResult Result { get; set; }
        public ushort FirstPlayerScore { get; set; }
        public ushort SecondPlayerScore { get; set; }
    }
}
