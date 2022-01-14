using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        private static string fieldsSql = "permission.*";
        private static string fromSql = "permissions as permission";
        private static string whereSql = "permission.excluded = false";
        private static string deleteSql = "UPDATE permissions SET excluded = true WHERE id = @id";
        private static string innerJoinPermissionGroup = "permission_groups AS permission_group ON permission.permission_group_id = permission_group.id";
        private static string innerJoinPermissionResource = "permission_resources AS permission_resource ON permission.permission_resource_id = permission_resource.id";

        public PermissionRepository(
            IDBContext dBContext, 
            IDataContext dataContext, 
            IDapperWrapper dapperWrapper, 
            IUtils utils
        ) : 
        base(
            dBContext, 
            dataContext, 
            dapperWrapper, 
            utils
        )
        {
            
        }

        public async Task<Permission> Create(Permission permission)
        {
            permission.id = Guid.NewGuid().ToString();

            await _dataContext.Permissions.AddAsync(permission);
            await _dataContext.SaveChangesAsync();

            return permission;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());

            return;
        }

        public Task<Permission> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResult<Permission>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string text = filter.text;
            string user_id = filter.user_id;

            string querySql = $@"
                SELECT {fieldsSql}, permission_resource.name as permission_resource_name FROM {fromSql} 
                INNER JOIN {innerJoinPermissionGroup} 
                INNER JOIN {innerJoinPermissionResource} 
                WHERE {whereSql} AND permission_group.user_id = @user_id
                AND permission_resource.name ILIKE @text
                LIMIT @limit OFFSET @offset
            ";

            Dictionary<string, object> _params = new Dictionary<string, object>();
            _params.Add("limit", limit);
            _params.Add("offset", offset);
            _params.Add("user_id", user_id);
            _params.Add("text", $"%{text}%");

            IEnumerable<Permission> permissions = await _dBContext.GetConnection().QueryAsync<Permission>(querySql, _params);
            int total_records = _dBContext.GetConnection().ExecuteScalar<int>($"SELECT COUNT(*) AS total_records FROM ({querySql}) permissions", _params);

            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);
            PaginationResult<Permission> paginationResult = _utils.CreatePaginationResult<Permission>(permissions.ToList(), paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<Permission> Update(Permission permission)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = permission;
            options.Where = "id = @id";
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();
            
            await _dapperWrapper.UpdateAsync<Permission>(options);
            return permission;
        }

        public async Task<Permission> Exists(string userId, string table)
        {
            string querySql = $@"
                SELECT {fieldsSql} FROM {fromSql}
                INNER JOIN {innerJoinPermissionGroup} 
                INNER JOIN {innerJoinPermissionResource}
                WHERE {whereSql} AND permission_group.user_id = @user_id
                AND permission_resource._table = @table LIMIT 1
            ";

            var queryParams = new Dictionary<string, object>();
            queryParams.Add("table", table);
            queryParams.Add("user_id", userId);

            Permission permission = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<Permission>(querySql, queryParams);

            return permission;
        }

        public async Task<bool> HasPermission(string userId, string table, string action)
        {
            string querySql = $@"
                SELECT permission._{action} AS has_permission FROM {fromSql}
                INNER JOIN {innerJoinPermissionGroup} 
                INNER JOIN {innerJoinPermissionResource}
                WHERE {whereSql} AND permission_group.user_id = @user_id
                AND permission_resource._table = @table LIMIT 1
            ";

            var queryParams = new Dictionary<string, object>();
            queryParams.Add("table", table);
            queryParams.Add("user_id", userId);

            bool hasPermission = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<bool>(querySql, queryParams);
            return hasPermission;
        }
    }
}