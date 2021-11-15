using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace clickfly.Repositories
{
    public class AccessTokenRepository : BaseRepository<AccessToken>, IAccessTokenRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "access_tokens as access_token";
        private static string whereSql = "access_token.excluded = false";
        private static string deleteSql = "UPDATE access_tokens SET excluded = true WHERE id = @id";

        public AccessTokenRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {

        }

        public async Task<AccessToken> Create(AccessToken accessToken)
        {
            string id = Guid.NewGuid().ToString();
            accessToken.id = id;

            await _dataContext.AccessTokens.AddAsync(accessToken);
            await _dataContext.SaveChangesAsync();

            return accessToken;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
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
