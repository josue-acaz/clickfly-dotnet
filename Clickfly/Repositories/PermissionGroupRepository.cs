using System;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;
using System.Collections.Generic;

namespace clickfly.Repositories
{
    public class PermissionGroupRepository : BaseRepository<PermissionGroup>, IPermissionGroupRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "permission_groups as permission_group";
        private static string whereSql = "permission_group.excluded = false";

        public PermissionGroupRepository(
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

        public async Task<PermissionGroup> Create(PermissionGroup permissionGroup)
        {
            permissionGroup.id = Guid.NewGuid().ToString();

            await _dataContext.PermissionGroups.AddAsync(permissionGroup);
            await _dataContext.SaveChangesAsync();

            return permissionGroup;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PermissionGroup> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<PermissionGroup> GetByUserId(string user_id)
        {
            SelectOptions options = new SelectOptions();
            options.As = "permission_group";
            options.Where = $"{whereSql} AND permission_group.user_id = @user_id";
            options.Params = new { user_id = user_id };

            options.Include<Permission>(new IncludeModel{
                As = "permissions",
                ForeignKey = "permission_group_id",
                Where = "permissions.excluded = false"
            });
            
            PermissionGroup permissionGroup = await _dapperWrapper.QuerySingleAsync<PermissionGroup>(options);
            return permissionGroup;
        }

        public Task<PaginationResult<PermissionGroup>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<PermissionGroup> Update(PermissionGroup permissionGroup)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = permissionGroup;
            options.Where = "id = @id";
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();
            
            await _dapperWrapper.UpdateAsync<PermissionGroup>(options);
            return permissionGroup;
        }
    }
}