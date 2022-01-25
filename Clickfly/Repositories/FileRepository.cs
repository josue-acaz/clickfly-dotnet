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
    public class FileRepository : BaseRepository<File>, IFileRepository 
    {
        private static string fieldsSql = "*";
        private static string fromSql = "files as file";
        private static string whereSql = "file.excluded = false";

        public FileRepository(IDBContext dBContext, IDataContext dataContext, IDapperWrapper dapperWrapper, IUtils utils) : base(dBContext, dataContext, dapperWrapper, utils)
        {

        }

        public async Task<File> Create(File file)
        {
            file.id = Guid.NewGuid().ToString();
            file.created_at = DateTime.Now;
            file.excluded = false;

            List<string> exclude = new List<string>();
            exclude.Add("updated_at");
            exclude.Add("updated_by");

            InsertOptions options = new InsertOptions();
            options.Data = file;
            options.Exclude = exclude;
            options.Transaction = _dBContext.GetTransaction();

            await _dapperWrapper.InsertAsync<File>(options);
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
