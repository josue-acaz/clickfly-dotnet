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
    public class UserRolePermissionRepository : BaseRepository<UserRole>, IUserRolePermissionRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "user_role_permissions as user_role_permission";
        private static string whereSql = "user_role_permission.excluded = false";

        public UserRolePermissionRepository(
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

        public Task<UserRolePermission> Create(UserRolePermission userRolePermission)
        {
            throw new NotImplementedException();
        }

        public Task<UserRolePermission> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginationResult<UserRolePermission>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
