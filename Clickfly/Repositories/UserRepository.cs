using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clickfly.Data;
using clickfly.Models;
using Npgsql;
using BCryptNet = BCrypt.Net.BCrypt;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private static string fieldsSql = "_user.*";
        private static string fromSql = "users as _user";
        private static string whereSql = "_user.excluded = false";
        private static string innerJoinPermissionGroup = "permission_groups AS permission_group ON permission_group.user_id = _user.id";
        private static string innerJoinUserRole = "user_roles AS user_role ON permission_group.user_role_id = user_role.id";
        
        public UserRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
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
            string querySql = $@"
                SELECT {fieldsSql}, user_role.label as role FROM {fromSql} 
                INNER JOIN {innerJoinPermissionGroup} 
                INNER JOIN {innerJoinUserRole} 
                WHERE {whereSql} AND _user.id = @id
            ";

            object param = new { id = id };
            User user = await _dBContext.GetConnection().QueryFirstOrDefaultAsync<User>(querySql, param);
            return user;
        }

        public async Task<PaginationResult<User>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string air_taxi_id = filter.air_taxi_id;

            string querySql = $@"
                SELECT {fieldsSql}, user_role.label as role FROM {fromSql} 
                INNER JOIN {innerJoinPermissionGroup} 
                INNER JOIN {innerJoinUserRole} 
                WHERE {whereSql} AND _user.air_taxi_id = @air_taxi_id
                LIMIT @limit OFFSET @offset
            ";

            Dictionary<string, object> _params = new Dictionary<string, object>();
            _params.Add("limit", limit);
            _params.Add("offset", offset);
            _params.Add("air_taxi_id", air_taxi_id);

            IEnumerable<User> users = await _dBContext.GetConnection().QueryAsync<User>(querySql, _params);
            int total_records = _dBContext.GetConnection().ExecuteScalar<int>($"SELECT COUNT(*) AS total_records FROM ({querySql}) users", _params);

            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);
            PaginationResult<User> paginationResult = _utils.CreatePaginationResult<User>(users.ToList(), paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<User> Update(User user)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = user;
            options.Where = "id = @id";
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.UpdateAsync<User>(options);
            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            string querySql = $@"
                SELECT {fieldsSql}, user_role.name as role FROM {fromSql} 
                LEFT OUTER JOIN {innerJoinPermissionGroup} 
                LEFT OUTER JOIN {innerJoinUserRole} 
                WHERE {whereSql} AND _user.email = @email
            ";

            object param = new { email = email };
            User user = await _dBContext.GetConnection().QueryFirstOrDefaultAsync<User>(querySql, param);
            return user;
        }

        public async Task<User> GetByUsername(string username)
        {
            string querySql = $@"
                SELECT {fieldsSql}, user_role.name as role FROM {fromSql} 
                LEFT OUTER JOIN {innerJoinPermissionGroup} 
                LEFT OUTER JOIN {innerJoinUserRole} 
                WHERE {whereSql} AND _user.username = @username
            ";

            object param = new { username = username };
            User user = await _dBContext.GetConnection().QueryFirstOrDefaultAsync<User>(querySql, param);
            return user;
        }
    }
}
