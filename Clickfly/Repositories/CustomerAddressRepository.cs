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
    public class CustomerAddressRepository : BaseRepository<CustomerAddress>, ICustomerAddressRepository 
    {
        private static string fieldsSql = "*";
        private static string fromSql = "customer_addresses as customer_address";
        private static string whereSql = "customer_address.excluded = false";

        public CustomerAddressRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<CustomerAddress> Create(CustomerAddress customerAddress)
        {
            customerAddress.id = Guid.NewGuid().ToString();

            await _dataContext.CustomerAddresses.AddAsync(customerAddress);
            await _dataContext.SaveChangesAsync();

            return customerAddress;
        }

        public async Task Delete(string id)
        {
            string querySql = $"UPDATE customer_addresses set excluded = true WHERE id = @id";
            object param = new { id = id };
            
            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
        }

        public async Task<CustomerAddress> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND customer_address.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            CustomerAddress customerAddress = await _dataContext.CustomerAddresses.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return customerAddress;
        }

        public async Task<PaginationResult<CustomerAddress>> Pagination(PaginationFilter filter)
        {
            string customerId = filter.customer_id;
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            List<CustomerAddress> customerAddresses = await _dataContext.CustomerAddresses
                .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
                .Take(paginationFilter.page_size)
                .Where(customerAddress => customerAddress.customer_id == customerId && customerAddress.excluded == false)
                .ToListAsync();
            
            int total_records = await _dataContext.CustomerAddresses.CountAsync();
            PaginationResult<CustomerAddress> paginationResult = _utils.CreatePaginationResult<CustomerAddress>(customerAddresses, paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<CustomerAddress> Update(CustomerAddress customerAddress)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = customerAddress;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<CustomerAddress>(options);
            return customerAddress;
        }
    }
}
