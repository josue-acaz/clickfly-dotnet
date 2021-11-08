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
        private readonly IOrm _orm;
        private static string fieldsSql = "*";
        private static string fromSql = "customers as customer";
        private static string whereSql = "customer.excluded = false";
        private static string subQueryThumbnailSql = $@"
            SELECT file.url AS thumbnail FROM files as file 
            WHERE file.excluded = false 
            AND file.resource = 'customers' 
            AND file.resource_id = customer.id
            AND file.field_name = 'thumbnail' 
            ORDER BY file.created_at 
            DESC LIMIT 1 OFFSET 0
        ";
        protected string[] defaultFields = new string[9];

        public CustomerRepository(IDBContext dBContext, IDataContext dataContext, IUtils utils, IOrm orm) : base(dBContext, dataContext, utils)
        {
            _orm = orm;

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
            string querySql = $@"SELECT {fieldsSql} ? {fromSql} WHERE {whereSql} AND customer.id = @id";

            Dictionary<string, object> queryParams = new Dictionary<string, object>();
            queryParams.Add("id", id);

            RawAttribute[] rawAttributes = new RawAttribute[1];
            RawAttribute thumbnail = new RawAttribute();
            thumbnail.name = "thumbnail";
            thumbnail.query = "SELECT url FROM files WHERE resource_id = customer.id AND resource = 'customers' AND field_name = 'thumbnail' LIMIT 1";
            rawAttributes[0] = thumbnail;

            QueryAsyncParams queryAsyncParams = new QueryAsyncParams();
            queryAsyncParams.tableName = "customers";
            queryAsyncParams.relationshipName = "customer";
            queryAsyncParams.rawAttributes = rawAttributes;
            queryAsyncParams.queryParams = queryParams;
            queryAsyncParams.querySql = querySql;

            Include includeCustomerCards = new Include();
            includeCustomerCards.tableName = "customer_cards";
            includeCustomerCards.relationshipName = "cards";
            includeCustomerCards.foreignKey = "customer_id";
            includeCustomerCards.where = "cards.excluded = false";
            includeCustomerCards.hasMany = true;

            Include includeCustomerAddresses = new Include();
            includeCustomerAddresses.tableName = "customer_addresses";
            includeCustomerAddresses.relationshipName = "addresses";
            includeCustomerAddresses.foreignKey = "customer_id";
            includeCustomerAddresses.where = "addresses.excluded = false";
            includeCustomerAddresses.hasMany = true;

            Include includeCustomerFriends = new Include();
            includeCustomerFriends.tableName = "customer_friends";
            includeCustomerFriends.relationshipName = "friends";
            includeCustomerFriends.foreignKey = "customer_id";
            includeCustomerFriends.where = "friends.excluded = false";
            includeCustomerFriends.hasMany = true;

            queryAsyncParams.includes.Add(includeCustomerCards);
            queryAsyncParams.includes.Add(includeCustomerAddresses);
            queryAsyncParams.includes.Add(includeCustomerFriends);

            Customer customer = await _orm.QuerySingleOrDefaultAsync<Customer>(queryAsyncParams);

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
