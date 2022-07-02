using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;
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
            SelectOptions options = new SelectOptions();
            options.As = "customer";
            options.Where = $"{whereSql} AND customer.email = @email";
            options.Params = new { email = email };

            Customer customer = await _dapperWrapper.QuerySingleAsync<Customer>(options);
            return customer;
        }

        public async Task<Customer> GetById(string id)
        {
            SelectOptions options = new SelectOptions();
            options.As = "customer";
            options.Where = $"excluded = false AND id = @id";
            options.Params = new { id = id };

            options.Include<CustomerCard>(new IncludeModel{
                As = "cards",
                ForeignKey = "customer_id",
                Where = "excluded = false"
            });

            options.Include<CustomerFriend>(new IncludeModel{
                As = "friends",
                ForeignKey = "customer_id",
                Where = "excluded = false"
            });

            options.Include<CustomerAddress>(new IncludeModel{
                As = "addresses",
                ForeignKey = "customer_id",
                Where = "excluded = false"
            });

            options.AddRawAttribute("thumbnail", @"
                SELECT url FROM files WHERE resource_id = customer.id 
                AND resource = 'customers' 
                AND field_name = 'thumbnail' 
                ORDER BY customer.created_at DESC LIMIT 1
            ");

            Customer customer = await _dapperWrapper.QuerySingleAsync<Customer>(options);

            return customer;
        }

        public async Task<Customer> GetByPasswordResetToken(string password_reset_token)
        {
            SelectOptions options = new SelectOptions();
            options.As = "customer";
            options.Where = $"{whereSql} AND customer.password_reset_token = @password_reset_token";
            options.Params = new { password_reset_token = password_reset_token };

            Customer customer = await _dapperWrapper.QuerySingleAsync<Customer>(options);
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
            List<string> exclude = new List<string>();
            exclude.Add("created_at");
            exclude.Add("created_by");
            exclude.Add("password_hash");

            UpdateOptions options = new UpdateOptions();
            options.Data = customer;
            options.Where = "id = @id";
            options.Transaction = _dBContext.GetTransaction();
            options.Exclude = exclude;

            await _dapperWrapper.UpdateAsync<Customer>(options);
            return customer;
        }

        public async Task UpdatePassword(Customer customer)
        {
            string querySql = $@"UPDATE customers SET password_hash = @password_hash, password_reset_token = '', password_reset_expires = null WHERE id = @id";
            string password_hash = BCryptNet.HashPassword(customer.password);

            object queryParams = new {
                id = customer.id,
                password_hash = password_hash,
            };

            await _dBContext.GetConnection().ExecuteAsync(querySql, queryParams, _dBContext.GetTransaction());
        }
    }
}
