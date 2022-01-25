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
    public class AccessTokenRepository : BaseRepository<AccessToken>, IAccessTokenRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "access_tokens as access_token";
        private static string whereSql = "access_token.excluded = false";
        private static string deleteSql = "UPDATE access_tokens SET excluded = true WHERE id = @id";

        public AccessTokenRepository(
            IDBContext dBContext, 
            IDataContext dataContext, 
            IDapperWrapper dapperWrapper, 
            IUtils utils
        ) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<AccessToken> Create(AccessToken accessToken)
        {
            accessToken.id = Guid.NewGuid().ToString();
            accessToken.created_at = DateTime.Now;
            accessToken.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = accessToken;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<AccessToken>(options);
            return accessToken;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public async Task<AccessToken> GetByToken(string token)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND access_token.token = @token";
            NpgsqlParameter param = new NpgsqlParameter("token", token);
            
            AccessToken accessToken = await _dataContext.AccessTokens.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return accessToken;
        }

        public Task<bool> TokenIsValid(string token)
        {
            throw new NotImplementedException();
        }
    }
}
