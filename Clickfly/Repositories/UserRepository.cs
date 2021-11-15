using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Npgsql;
using BCryptNet = BCrypt.Net.BCrypt;
using Dapper;

namespace clickfly.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "users as _user";
        private static string whereSql = "_user.excluded = false";

        public UserRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {
            
        }

        public async Task<User> Create(User user)
        {
            string id = Guid.NewGuid().ToString();
            string password = user.password;
            string passwordHash = BCryptNet.HashPassword(password);

            user.id = id;
            user.password_hash = passwordHash;

            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            user.password = "";
            
            return user;
        }   

        public async Task Delete(string id)
        {
            string querySql = $"UPDATE users as _user set _user.excluded = true WHERE _user.id = @id";
            object param = new { id = id };
            
            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
        }

        public async Task<User> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND _user.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            User user = await _dataContext.Users.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return user;
        }

        public Task<PaginationResult<User>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task Update(User user)
        {   
            throw new NotImplementedException();
        }

        public async Task<User> GetByEmail(string email)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND _user.email = @email";
            NpgsqlParameter param = new NpgsqlParameter("email", email);
            
            User user = await _dataContext.Users.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetByUsername(string username)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND _user.username = @username";
            NpgsqlParameter param = new NpgsqlParameter("username", username);
            
            User user = await _dataContext.Users.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return user;
        }
    }
}
