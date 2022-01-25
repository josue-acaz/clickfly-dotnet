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
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "user_roles as user_role";
        private static string whereSql = "user_role.excluded = false";

        public UserRoleRepository(
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

        public async Task<UserRole> Create(UserRole userRole)
        {
            userRole.id = Guid.NewGuid().ToString();
            userRole.created_at = DateTime.Now;
            userRole.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = userRole;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<UserRole>(options);
            return userRole;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserRole> GetByName(string name)
        {
            SelectOptions options = new SelectOptions();
            options.As = "user_role";
            options.Params = new { name = name };
            options.Where = $"{whereSql} AND user_role.name = @name";
            
            options.Include<UserRolePermission>(new IncludeModel{
                As = "permissions",
                ForeignKey = "user_role_id",
                Where = "permissions.excluded = false"
            });
            
            UserRole userRole = await _dapperWrapper.QuerySingleAsync<UserRole>(options);
            return userRole;
        }

        public async Task<UserRole> GetByUserId(string userId)
        {
            SelectOptions options = new SelectOptions();
            options.As = "user_role";
            options.Params = new { user_id = userId };
            options.Where = $"{whereSql} AND permission_group.user_id = @user_id";

            options.Include<PermissionGroup>(new IncludeModel{
                As = "permission_group",
                ForeignKey = "user_role_id",
                Where = "permission_group.excluded = false",
            });

            options.Include<UserRolePermission>(new IncludeModel{
                As = "permissions",
                ForeignKey = "user_role_id",
                Where = "permissions.excluded = false"
            });
            
            UserRole userRole = await _dapperWrapper.QuerySingleAsync<UserRole>(options);
            return userRole;
        }

        public async Task<PaginationResult<UserRole>> Pagination(PaginationFilter filter)
        {
            int limit = filter.page_size;
            int offset = (filter.page_number - 1) * filter.page_size;
            string text = filter.text;

            string where = $"{whereSql} AND user_role.name ILIKE @text";

            filter.exclude.ForEach(ex => {
                where += $" AND user_role.{ex.name} != '{ex.value}' ";
            });

            where += " LIMIT @limit OFFSET @offset ";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("limit", limit);
            queryParams.Add("offset", offset);
            queryParams.Add("text", $"%{text}%");

            SelectOptions options = new SelectOptions();
            options.As = "user_role";
            options.Where = where;
            options.Params = queryParams;

            IEnumerable<UserRole> user_roles = await _dapperWrapper.QueryAsync<UserRole>(options);
            int total_records = _dapperWrapper.Count<UserRole>(new CountOptions {
                Where = where,
                Params = queryParams,
                As = "user_role"
            });

            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);
            PaginationResult<UserRole> paginationResult = _utils.CreatePaginationResult<UserRole>(user_roles.ToList(), paginationFilter, total_records);

            return paginationResult;
        }
    }
}