using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Blorem.Presentation.Main.Caching;
using System;

namespace Blorem.Presentation.Main.Hubs
{

    public class MainHub : Hub
    {

        private readonly ICacheManager _cache;

        public MainHub()
        {
            var wrapper = new RedisConnectionWrapper("redisblorem");
            _cache = new RedisCacheManager(wrapper);
        }

        public async Task ChooseState(string state)
        {
            _cache.Set(state, DateTime.Now.ToString(), 50);
            await Clients.All.SendAsync("ReceiveState", state);
        }
    }

}