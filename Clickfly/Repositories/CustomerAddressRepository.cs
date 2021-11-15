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
    public class CustomerAddressRepository : BaseRepository<CustomerAddress>, ICustomerAddressRepository 
    {
        private static string fieldsSql = "*";
        private static string fromSql = "customer_addresses as customer_address";
        private static string whereSql = "customer_address.excluded = false";
        protected string[] defaultFields = new string[10];

        public CustomerAddressRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {
            defaultFields[0] = "name";
            defaultFields[1] = "street";
            defaultFields[2] = "number";
            defaultFields[3] = "zipcode";
            defaultFields[4] = "neighborhood";
            defaultFields[5] = "state";
            defaultFields[6] = "city";
            defaultFields[7] = "address_id";
            defaultFields[8] = "complement";
            defaultFields[9] = "customer_id";
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

        public async Task<CustomerAddress> Update(CustomerAddress customerAddress, string[] fields = null)
        {
            if(fields == null)
            {
                fields = defaultFields;
            }

            GetFieldsSqlParams fieldsSqlParams = new GetFieldsSqlParams();
            fieldsSqlParams.action = "UPDATE";
            fieldsSqlParams.fields = fields;

            string fieldsToUpdate = _utils.GetFieldsSql(fieldsSqlParams);
            string querySql = $"UPDATE customer_addresses SET {fieldsToUpdate} WHERE id = @id";

            await _dBContext.GetConnection().ExecuteAsync(querySql, customerAddress, _dBContext.GetTransaction());

            return customerAddress;
        }
    }
}
