using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using clickfly.Data;
using clickfly.Models;
using clickfly.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using clickfly.ViewModels;

namespace clickfly.Repositories
{
    public class AerodromeRepository : BaseRepository<Aerodrome>, IAerodromeRepository
    {
        private static string fieldsSql = "*";
        private static string fromSql = "aerodromes as aerodrome";
        private static string whereSql = "aerodrome.excluded = false";

        public AerodromeRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<Aerodrome> Create(Aerodrome aerodrome)
        {
            string id = Guid.NewGuid().ToString();
            aerodrome.id = id;

            await _dataContext.Aerodromes.AddAsync(aerodrome);
            await _dataContext.SaveChangesAsync();

            return aerodrome;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Aerodrome> GetById(string id)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND aerodrome.id = @id";
            NpgsqlParameter param = new NpgsqlParameter("id", id);
            
            Aerodrome aerodrome = await _dataContext.Aerodromes.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            
            return aerodrome;
        }

        public async Task<PaginationResult<Aerodrome>> Pagination(PaginationFilter filter)
        {
            PaginationFilter paginationFilter= new PaginationFilter(filter.page_number, filter.page_size);

            List<Aerodrome> aerodromes = await _dataContext.Aerodromes
               .Skip((paginationFilter.page_number - 1) * paginationFilter.page_size)
               .Take(paginationFilter.page_size)
               .ToListAsync();
            
            int total_records = await _dataContext.Aerodromes.CountAsync();
            PaginationResult<Aerodrome> paginationResult = _utils.CreatePaginationResult<Aerodrome>(aerodromes, paginationFilter, total_records);

            return paginationResult;
        }

        public Task Update(Aerodrome aerodrome)
        {
            throw new NotImplementedException();
        }
    }
}