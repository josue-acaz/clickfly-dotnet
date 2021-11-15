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
    public class FileRepository : BaseRepository<File>, IFileRepository 
    {
        private static string fieldsSql = "*";
        private static string fromSql = "files as file";
        private static string whereSql = "file.excluded = false";
        protected string[] defaultFields = new string[8];

        public FileRepository(IDBContext dBContext, IDataContext dataContext, IDBAccess dBAccess, IUtils utils) : base(dBContext, dataContext, dBAccess, utils)
        {

        }

        public async Task<File> Create(File file)
        {
            string id = Guid.NewGuid().ToString();
            file.id = id;

            await _dataContext.Files.AddAsync(file);
            await _dataContext.SaveChangesAsync();

            return file;
        }

        public async Task Delete(string id)
        {
            string querySql = $"UPDATE files as file set file.excluded = true WHERE file.id = @id";
            object param = new { id = id };
            
            await _dBContext.GetConnection().ExecuteAsync(querySql, param, _dBContext.GetTransaction());
        }

        public async Task<File> GetByFieldName(string fieldName)
        {
            string querySql = $"SELECT {fieldsSql} FROM {fromSql} WHERE {whereSql} AND file.field_name ILIKE @fieldName";
            NpgsqlParameter param = new NpgsqlParameter("fieldName", $"%{fieldName}%");
            
            File file = await _dataContext.Files.FromSqlRaw(querySql, param).FirstOrDefaultAsync();
            return file;
        }
    }
}
