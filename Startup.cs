using Owin;
using Microsoft.Owin;
namespace BlackJack
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
