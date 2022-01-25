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
    public class CustomerFriendRepository : BaseRepository<CustomerFriend>, ICustomerFriendRepository 
    {
        private static string fieldsSql = "*";
        private static string fromSql = "customer_friends as customer_friend";
        private static string whereSql = "customer_friend.excluded = false";

        public CustomerFriendRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

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
            customerFriend.created_at = DateTime.Now;
            customerFriend.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = customerFriend;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<CustomerFriend>(options);
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

        public async Task<CustomerFriend> Update(CustomerFriend customerFriend)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = customerFriend;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<CustomerFriend>(options);
            return customerFriend;
        }
    }
}