using System;
using System.Data;

namespace clickfly.Data
{
    public interface IDBContext : IDisposable
    {
        IDbConnection GetConnection();
        ConnectionState GetConnectionState();
        IDbTransaction GetTransaction();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
