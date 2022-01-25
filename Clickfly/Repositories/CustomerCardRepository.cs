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
    public class CustomerCardRepository : BaseRepository<CustomerCard>, ICustomerCardRepository 
    {
        private static string fieldsSql = "*";
        private static string fromSql = "customer_cards as customer_card";
        private static string whereSql = "customer_card.excluded = false";

        public CustomerCardRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<CustomerCard> Create(CustomerCard customerCard)
        {
            customerCard.id = Guid.NewGuid().ToString();
            customerCard.created_at = DateTime.Now;
            customerCard.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = customerCard;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<CustomerCard>(options);
            return customerCard;
        }

        public async Task Delete(string id)
        {
            string querySql = $"UPDATE customer_cards set excluded = true WHERE id = @id";
            object param = new { id = id };
            
            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
        }

        public async Task<CustomerCard> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND customer_card.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            CustomerCard customerCard = await _dataContext.CustomerCards.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return customerCard;
        }

        public async Task<PaginationResult<CustomerCard>> Pagination(PaginationFilter filter)
        {
            string customerId = filter.customer_id;
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            List<CustomerCard> customerCards = await _dataContext.CustomerCards
                .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
                .Take(paginationFilter.page_size)
                .Where(customerCard => customerCard.customer_id == customerId && customerCard.excluded == false)
                .ToListAsync();
            
            int total_records = await _dataContext.CustomerCards.CountAsync();
            PaginationResult<CustomerCard> paginationResult = _utils.CreatePaginationResult<CustomerCard>(customerCards, paginationFilter, total_records);

            return paginationResult;
        }

        public async Task<CustomerCard> Update(CustomerCard customerCard)
        {
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");

            UpdateOptions options = new UpdateOptions();
            options.Data = customerCard;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<CustomerCard>(options);
            return customerCard;
        }
    }
}