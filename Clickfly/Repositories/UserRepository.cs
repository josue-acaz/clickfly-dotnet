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

        public async Task UpdatePassword(string user_id, string password)
        {

            string querySql = $"UPDATE users SET password_hash = @password_hash WHERE id = @user_id";
            string password_hash = BCryptNet.HashPassword(password);

            object queryParams = new {
                user_id = user_id,
                password_hash = password_hash,
            };

            await _dBContext.GetConnection().ExecuteAsync(querySql, queryParams, _dBContext.GetTransaction());
        }

        public async Task Delete(string id)
        {
            string querySql = $"UPDATE users as _user set _user.excluded = true WHERE _user.id = @id";
            object param = new { id = id };
            
            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
        }

        public async Task<User> GetById(string id)
        {
            IncludeModel includePermissionGroup = new IncludeModel();
            includePermissionGroup.As = "permission_group";
            includePermissionGroup.ForeignKey = "user_id";
            includePermissionGroup.ThenInclude<UserRole>(new IncludeModel{
                As = "user_role",
                ForeignKey = "user_role_id",
            });

            SelectOptions options = new SelectOptions();
            options.As = "_user";
            options.Where = $"{whereSql} AND _user.id = @id";
            options.Params = new { id = id };

            options.AddRawAttribute("role", "user_role.name");
            options.Include<PermissionGroup>(includePermissionGroup);

            User user = await _dapperWrapper.QuerySingleAsync<User>(options);
            return user;
        }

        public async Task<PaginationResult<User>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string air_taxi_id = filter.air_taxi_id;

            string where = $"{whereSql} AND _user.air_taxi_id = @air_taxi_id";
            
            filter.exclude.ForEach(ex => {
                where += $" AND user_role.{ex.name} != '{ex.value}' ";
            });
            
            where += " LIMIT @limit OFFSET @offset ";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("air_taxi_id", air_taxi_id);

            IncludeModel includePermissionGroup = new IncludeModel();
            includePermissionGroup.As = "permission_group";
            includePermissionGroup.ForeignKey = "user_id";
            includePermissionGroup.ThenInclude<UserRole>(new IncludeModel{
                As = "user_role",
                ForeignKey = "user_role_id",
            });

            SelectOptions options = new SelectOptions();
            options.As = "_user";
            options.Where = where;
            options.Params = queryParams;
            options.Include<PermissionGroup>(includePermissionGroup);
            options.AddRawAttribute("role", "user_role.label");

            IEnumerable<User> users = await _dapperWrapper.QueryAsync<User>(options);
            int total_records = users.Count();

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
            object param = new { email = email };

            IncludeModel includePermissionGroup = new IncludeModel();
            includePermissionGroup.As = "permission_group";
            includePermissionGroup.ForeignKey = "user_id";
            includePermissionGroup.ThenInclude<UserRole>(new IncludeModel{
                As = "user_role",
                ForeignKey = "user_role_id",
            });
            
            SelectOptions options = new SelectOptions();
            options.As = "_user";
            options.Params = param;
            options.Where = $"{whereSql} AND _user.email = @email";

            options.Include<PermissionGroup>(includePermissionGroup);
            options.Include<AirTaxi>(new IncludeModel{
                As = "air_taxi",
                ForeignKey = "air_taxi_id",
            });

            options.AddRawAttribute("role", "user_role.name");

            User user = await _dapperWrapper.QuerySingleAsync<User>(options);
            return user;
        }

        public async Task<User> GetByUsername(string username)
        {
            object param = new { username = username };

            IncludeModel includePermissionGroup = new IncludeModel();
            includePermissionGroup.As = "permission_group";
            includePermissionGroup.ForeignKey = "user_id";
            includePermissionGroup.ThenInclude<UserRole>(new IncludeModel{
                As = "user_role",
                ForeignKey = "user_role_id",
            });
            
            SelectOptions options = new SelectOptions();
            options.As = "_user";
            options.Params = param;
            options.Where = $"{whereSql} AND _user.username = @username";

            options.Include<PermissionGroup>(includePermissionGroup);
            options.Include<AirTaxi>(new IncludeModel{
                As = "air_taxi",
                ForeignKey = "air_taxi_id",
            });

            options.AddRawAttribute("role", "user_role.name");

            User user = await _dapperWrapper.QuerySingleAsync<User>(options);
            return user;
        }
    }
}
