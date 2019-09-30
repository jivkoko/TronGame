using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TronGameCore.API.Models.Responses
{
    public class TurnMovedResponse : GameResponse
    {
        public int FirstPlayerRow { get; set; }
        public int FirstPlayerColumn { get; set; }

        public int SecondPlayerRow { get; set; }
        public int SecondPlayerColumn { get; set; }
    }
}
