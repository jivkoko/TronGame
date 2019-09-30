using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TronGameCore.API.Games;

namespace TronGameCore.API.Models.Requests
{
    public class KeyPressedRequest : GameRequest
    {
        public Keys KeyPressed { get; set; }
    }
}
