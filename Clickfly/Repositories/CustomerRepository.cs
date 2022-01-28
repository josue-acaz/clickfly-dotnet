using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Linq;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "customers as customer";
        private static string whereSql = "customer.excluded = false";
        private static string deleteSql = "UPDATE customers SET excluded = true WHERE id = @id";
        private static string customerThumbnailSql = $@"SELECT url FROM files WHERE resource_id = customer.id AND resource = 'customers' AND field_name = 'thumbnail' LIMIT 1";

        public CustomerRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<Customer> Create(Customer customer)
        {
            customer.id = Guid.NewGuid().ToString();
            customer.created_at = DateTime.Now;
            customer.excluded = false;
            customer.verified = true;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = customer;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<Customer>(options);
            return customer;
        }

        public async Task Delete(string id)
        {
            object param = new { id = id };
            await _dBContext.GetConnection().ExecuteAsync(deleteSql, param, _dBContext.GetTransaction());
        }

        public async Task<Customer> GetByEmail(string email)
        {
            Customer customer = await _dataContext.Customers
            .FirstOrDefaultAsync(customer => customer.email == email && customer.excluded == false);

            return customer;
        }

        public async Task<Customer> GetById(string id)
        {
            Customer customer = await _dataContext.Customers
            .Include(customer => customer.cards.Where(card => card.excluded == false))
            .Include(customer => customer.friends.Where(friend => friend.excluded == false))
            .Include(customer => customer.addresses.Where(address => address.excluded == false))
            .FirstOrDefaultAsync(customer => customer.id == id && customer.excluded == false);

            return customer;
        }

        public async Task<Customer> GetByPasswordResetToken(string password_reset_token)
        {
            Customer customer = await _dataContext.Customers
            .FirstOrDefaultAsync(customer => customer.password_reset_token == password_reset_token && customer.excluded == false);

            return customer;
        }

        public async Task MarkAsVerified(string id)
        {
            string querySql = $"UPDATE {fromSql} WHERE {whereSql} AND customer.id = @id AND customer.verified = false";
            object param = new { id = id };

            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
            return;
        }

        public Task<PaginationResult<Customer>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PasswordResetTokenIsValid(string password_reset_token)
        {
            DateTime now = DateTime.Now;

            string querySql = $@"
                select customer.password_reset_expires >= @now as isValid from {fromSql}
                where {whereSql} customer.password_reset_token = @password_reset_token
            ";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("@now", now);
            queryParams.Add("@password_reset_token", password_reset_token);

            bool isValid = await _dBContext.GetConnection().QuerySingleOrDefaultAsync<bool>(querySql, queryParams, _dBContext.GetTransaction());

            return isValid;
        }

        public async Task<Customer> Update(Customer customer)
        {
            var entity = await _dataContext.Customers.FirstAsync(c => c.id == customer.id);

            if(entity != null)
            {
                if(entity.name != customer.name)
                {
                    entity.name = customer.name;
                }

                if(entity.email != customer.email)
                {
                    entity.email = customer.email;
                }

                if(entity.document_type != customer.document_type)
                {
                    entity.document_type = customer.document_type;
                }
                
                if(entity.document != customer.document)
                {
                    entity.document = customer.document;
                }
                
                if(entity.birthdate != customer.birthdate)
                {
                    entity.birthdate = customer.birthdate;
                }
                
                if(entity.phone_number != customer.phone_number)
                {
                    entity.phone_number = customer.phone_number;
                }
                
                if(entity.emergency_phone_number != customer.emergency_phone_number)
                {
                    entity.emergency_phone_number = customer.emergency_phone_number;
                }

                await _dataContext.SaveChangesAsync();
            }

            return customer;
        }
    }
}
