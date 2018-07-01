using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Blorem.Presentation.Main.Hubs
{

    public class MainHub : Hub
    {
        public async Task ChooseState(string state)
        {
            await Clients.All.SendAsync("ReceiveState", state);
        }
    }

}