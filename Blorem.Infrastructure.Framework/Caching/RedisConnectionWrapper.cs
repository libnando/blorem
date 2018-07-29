using System;
using System.Linq;
using System.Net;
using StackExchange.Redis;

namespace Blorem.Infrastructure.Framework.Caching
{
    
    public class RedisConnectionWrapper : IRedisConnectionWrapper
    {
        
        private readonly object _lock = new object();
        private volatile ConnectionMultiplexer _connection;
        private readonly string _connectionString;

        public RedisConnectionWrapper(string conn)
        {
            this._connectionString = conn;            
        }

        protected ConnectionMultiplexer GetConnection()
        {
            if (_connection != null && _connection.IsConnected) return _connection;

            lock (_lock)
            {
                if (_connection != null && _connection.IsConnected) return _connection;

                _connection?.Dispose();

                _connection = ConnectionMultiplexer.Connect(_connectionString);
            }

            return _connection;
        }

        public IDatabase GetDatabase(int? db = null)
        {
            return GetConnection().GetDatabase(db ?? -1);
        }

        public IServer GetServer(EndPoint endPoint)
        {
            return GetConnection().GetServer(endPoint);
        }

        public EndPoint[] GetEndPoints()
        {
            return GetConnection().GetEndPoints();
        }

        public void FlushDatabase(int? db = null)
        {
            var endPoints = GetEndPoints();

            foreach (var endPoint in endPoints)
            {
                GetServer(endPoint).FlushDatabase(db ?? -1);
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

    }
}