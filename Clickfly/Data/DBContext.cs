using System;
using System.Data;

namespace clickfly.Data
{
    public class DBContext : IDBContext
    {
        public IDbConnection _dbConnection;
        public IDbTransaction _dbTransaction;

        public DBContext(IDbConnection dbConnection, IDbTransaction dbTransaction = null)
        {
            _dbConnection = dbConnection;
            _dbTransaction = dbTransaction;
        }

        public void BeginTransaction()
        {
            if(_dbTransaction == null)
            {
                _dbTransaction = _dbConnection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            if(_dbTransaction != null)
            {
                _dbTransaction.Commit();
                _dbTransaction = null;
            }
        }

        public void Dispose()
        {
            _dbConnection.Close();
            _dbConnection.Dispose();
            GC.SuppressFinalize(this);
        }

        public ConnectionState GetConnectionState()
        {
            return _dbConnection.State;
        }

        public IDbConnection GetConnection()
        {
            if(_dbConnection.State != ConnectionState.Open) _dbConnection.Open();

            return _dbConnection;
        }

        public IDbTransaction GetTransaction()
        {
            return _dbTransaction;
        }

        public void RollbackTransaction()
        {
            if(_dbTransaction != null)
            {
                _dbTransaction.Rollback();
                _dbTransaction = null;
            }
        }
    }
}