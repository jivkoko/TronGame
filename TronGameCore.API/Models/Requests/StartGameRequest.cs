using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TronGameCore.API.Models.Requests
{
    public class StartGameRequest : GameRequest
    {
        public ushort BorderWidth { get; set; }

        public ushort BorderHeight { get; set; }
    }
}