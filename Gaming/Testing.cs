using BlackJack.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Web.Routing;
using Microsoft.AspNet.SignalR.Hubs;

namespace BlackJack.Gaming
{
    public class Testing
    {
        ChatHub hub;
        public Testing(ChatHub hub) {
            this.hub = hub;
        }
    }
}
