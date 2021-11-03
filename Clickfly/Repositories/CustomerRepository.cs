using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;
using Npgsql;

namespace clickfly.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "customers as customer";
        private static string whereSql = "customer.excluded = false";
        private static string subQueryThumbnailSql = $@"
            SELECT file.url AS thumbnail FROM files as file 
            WHERE file.excluded = false 
            AND file.resource = '{Resources.Customers}' 
            AND file.resource_id = customer.id
            AND file.field_name = '{FieldNames.Thumbnail}' 
            ORDER BY file.created_at 
            DESC LIMIT 1 OFFSET 0
        ";
        protected string[] defaultFields = new string[9];

        public CustomerRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils) : base(dBContext, dataContext, utils)
        {
            defaultFields[0] = "name";
            defaultFields[1] = "email";
            defaultFields[2] = "phone_number";
            defaultFields[3] = "emergency_phone_number";
            defaultFields[4] = "document";
            defaultFields[5] = "document_type";
            defaultFields[6] = "type";
            defaultFields[7] = "birthdate";
            defaultFields[8] = "customer_id";
        }

        public async Task<Customer> Create(Customer customer)
        {
            string id = Guid.NewGuid().ToString();
            string password = customer.password;

            customer.id = id;
            customer.password_hash = BCryptNet.HashPassword(password);
            customer.role = "customer";
            customer.verified = false;

            await _dataContext.Customers.AddAsync(customer);
            await _dataContext.SaveChangesAsync();

            return customer;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> GetByEmail(string email)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND customer.email ILIKE @email";
            NpgsqlParameter param = new NpgsqlParameter("email", $"%{email}%");
            
            Customer customer = await _dataContext.Customers.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return customer;
        }

        public async Task<Customer> GetById(string id)
        {
            string querySql = $@"SELECT {fieldsSql}, ({subQueryThumbnailSql}) AS thumbnail FROM {fromSql} WHERE {whereSql} AND customer.id = @id";

            NpgsqlParameter param = new NpgsqlParameter("id", id);

            Customer customer = await _dataContext.Customers
            .FromSqlRaw(querySql, param)
            .Include(customer => customer.cards)
            .Include(customer => customer.addresses)
            .Include(customer => customer.friends)
            .FirstOrDefaultAsync();

            return customer;
        }

        public async Task<Customer> GetByPasswordResetToken(string password_reset_token)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND customer.password_reset_token = @password_reset_token";
            NpgsqlParameter param = new NpgsqlParameter("password_reset_token", password_reset_token);
            
            Customer customer = await _dataContext.Customers.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
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

        public async Task<Customer> Update(Customer customer, string[] fields = null)
        {
            if(fields == null)
            {
                fields = defaultFields;
            }

            GetFieldsSqlParams fieldsSqlParams = new GetFieldsSqlParams();
            fieldsSqlParams.action = "UPDATE";
            fieldsSqlParams.fields = fields;

            string fieldsToUpdate = _utils.GetFieldsSql(fieldsSqlParams);
            string querySql = $"UPDATE customers SET {fieldsToUpdate} WHERE id = @id";

            await _dBContext.GetConnection().ExecuteAsync(querySql, customer, _dBContext.GetTransaction());

            return customer;
        }
    }
}
