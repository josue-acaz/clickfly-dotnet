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
    public class PermissionResourceRepository : BaseRepository<PermissionResource>, IPermissionResourceRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "permission_resources as permission_resource";
        private static string whereSql = "permission_resource.excluded = false";

        public PermissionResourceRepository(
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

        public async Task<PermissionResource> Create(PermissionResource permissionResource)
        {
            string id = Guid.NewGuid().ToString();
            permissionResource.id = id;

            await _dataContext.PermissionResources.AddAsync(permissionResource);
            await _dataContext.SaveChangesAsync();

            return permissionResource;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PermissionResource> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND permission_resource.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);

            PermissionResource permissionResource = await _dataContext.PermissionResources
            .FromSqlRaw<PermissionResource>(querySql, param)
            .FirstOrDefaultAsync();

            return permissionResource;
        }

        public async Task<PermissionResource> GetByName(string name)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND permission_resource.name = @name";
            NpgsqlParameter param = new NpgsqlParameter("name", name);
            
            PermissionResource permissionResource = await _dataContext.PermissionResources
            .FromSqlRaw(querySql, param)
            .FirstOrDefaultAsync();

            return permissionResource;
        }

        public async Task<PaginationResult<PermissionResource>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string text = filter.text;

            string querySql = $@"
                SELECT {fieldsSql} FROM {fromSql} 
                WHERE {whereSql} AND permission_resource.name ILIKE @text
                LIMIT @limit OFFSET @offset
            ";

            Dictionary<string, object> _params = new Dictionary<string, object>();
            _params.Add("limit", limit);
            _params.Add("offset", offset);
            _params.Add("text", $"%{text}%");

            IEnumerable<PermissionResource> permission_resources = await _dBContext.GetConnection().QueryAsync<PermissionResource>(querySql, _params);
            int total_records = _dBContext.GetConnection().ExecuteScalar<int>($"SELECT COUNT(*) AS total_records FROM ({querySql}) permission_resources", _params);

            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);
            PaginationResult<PermissionResource> paginationResult = _utils.CreatePaginationResult<PermissionResource>(permission_resources.ToList(), paginationFilter, total_records);

            return paginationResult;
        }

        public Task Update(PermissionResource permissionResource)
        {
            throw new NotImplementedException();
        }
    }
}