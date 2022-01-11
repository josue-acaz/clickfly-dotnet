using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class AccountVerificationRepository : BaseRepository<AccountVerification>, IAccountVerificationRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "account_verifications as account_verification";
        private static string whereSql = "account_verification.excluded = false";
        private static string deleteSql = "UPDATE account_verification SET excluded = true WHERE id = @id";

        public AccountVerificationRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<AccountVerification> Create(AccountVerification accountVerification)
        {
            string id = Guid.NewGuid().ToString();
            accountVerification.id = id;

            await _dataContext.AccountVerifications.AddAsync(accountVerification);
            await _dataContext.SaveChangesAsync();

            return accountVerification;
        }

        public async Task<AccountVerification> GetByToken(string token)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND account_verification.token = @token";
            NpgsqlParameter param = new NpgsqlParameter("token", token);
            
            AccountVerification accountVerification = await _dataContext.AccountVerifications.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return accountVerification;
        }

        public async Task<bool> TokenIsValid(string token)
        {
            DateTime now = DateTime.Now;

            string querySql = $@"
                select account_verification.expires >= @now as isValid from {fromSql}
                where {whereSql} account_verification.token = @token
            ";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("@now", now);
            queryParams.Add("@token", token);

            bool isValid = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<bool>(querySql, queryParams, _dBContext.GetTransaction());

            return isValid;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());

            return;
        }
    }
}
