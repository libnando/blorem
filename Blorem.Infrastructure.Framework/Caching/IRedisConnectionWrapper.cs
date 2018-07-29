using System;
using System.Net;
using StackExchange.Redis;

namespace Blorem.Infrastructure.Framework.Caching
{

    public interface IRedisConnectionWrapper : IDisposable
    {

        IDatabase GetDatabase(int? db = null);

        IServer GetServer(EndPoint endPoint);

        EndPoint[] GetEndPoints();

        void FlushDatabase(int? db = null);
    }
}
