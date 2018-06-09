using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace blorem.Hubs{

public class MainHub : Hub
{
    public async Task ChooseState(string state)
    {
        await Clients.All.SendAsync("ReceiveState", state);
    }
}

}