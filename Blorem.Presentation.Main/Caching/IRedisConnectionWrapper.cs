using System;
using System.Net;
using StackExchange.Redis;

namespace Blorem.Presentation.Main.Caching
{

    public interface IRedisConnectionWrapper : IDisposable
    {

        IDatabase GetDatabase(int? db = null);

        IServer GetServer(EndPoint endPoint);

        EndPoint[] GetEndPoints();

        void FlushDatabase(int? db = null);
    }
}
