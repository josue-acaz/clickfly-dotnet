using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace clickfly.Repositories
{
    public class CustomerFriendRepository : BaseRepository<CustomerFriend>, ICustomerFriendRepository 
    {
        private static string fieldsSql = "*";
        private static string fromSql = "customer_friends as customer_friend";
        private static string whereSql = "customer_friend.excluded = false";
        protected string[] defaultFields = new string[8];

        public CustomerFriendRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils) : base(dBContext, dataContext, utils)
        {
            defaultFields[0] = "name";
            defaultFields[1] = "email";
            defaultFields[2] = "phone_number";
            defaultFields[3] = "emergency_phone_number";
            defaultFields[4] = "document";
            defaultFields[5] = "document_type";
            defaultFields[6] = "birthdate";
            defaultFields[7] = "customer_id";
        }

        public async Task<IEnumerable<CustomerFriend>> BulkGetById(string[] ids)
        {
            string bulkSql = _utils.GetBulkSql(ids);
            object param = new { bulkSql = bulkSql }; // Não é usado

            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND customer_friend.id = ANY('{bulkSql}')";

            IEnumerable<CustomerFriend> customerFriends = await _dBContext.GetConnection().QueryAsync<CustomerFriend>(querySql, param);
            return customerFriends;
        }

        public async Task<CustomerFriend> Create(CustomerFriend customerFriend)
        {
            customerFriend.id = Guid.NewGuid().ToString();

            await _dataContext.CustomerFriends.AddAsync(customerFriend);
            await _dataContext.SaveChangesAsync();

            return customerFriend;
        }

        public async Task Delete(string id)
        {
            string querySql = $"UPDATE customer_friends set excluded = true WHERE id = @id";
            object param = new { id = id };
            
            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
        }

        public async Task<CustomerFriend> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND customer_friend.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            CustomerFriend customerFriend = await _dataContext.CustomerFriends.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return customerFriend;
        }

        public async Task<PaginationResult<CustomerFriend>> Pagination(PaginationFilter filter)
        {
            string customerId = filter.customer_id;
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            List<CustomerFriend> customerFriends = await _dataContext.CustomerFriends
               .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
               .Take(paginationFilter.page_size)
               .Where(customerFriend => customerFriend.customer_id == customerId && customerFriend.excluded == false)
               .ToListAsync();
            
            int total_records = await _dataContext.CustomerFriends.CountAsync();
            PaginationResult<CustomerFriend> paginationResult = _utils.CreatePaginationResult<CustomerFriend>(customerFriends, paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<CustomerFriend> Update(CustomerFriend customerFriend, string[] fields = null)
        {
            if(fields == null)
            {
                fields = defaultFields;
            }

            GetFieldsSqlParams fieldsSqlParams = new GetFieldsSqlParams();
            fieldsSqlParams.action = "UPDATE";
            fieldsSqlParams.fields = fields;

            string fieldsToUpdate = _utils.GetFieldsSql(fieldsSqlParams);
            string querySql = $"UPDATE customer_friends SET {fieldsToUpdate} WHERE id = @id";

            await _dBContext.GetConnection().ExecuteAsync(querySql, customerFriend, _dBContext.GetTransaction());

            return customerFriend;
        }
    }
}