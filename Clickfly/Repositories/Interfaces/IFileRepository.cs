using System.Collections.Generic;
using System.Threading.Tasks;
using clickfly.ViewModels;
using clickfly.Models;

namespace clickfly.Repositories
{
    public interface IFileRepository
    {
        Task<File> Create(File file);
        Task<File> GetByFieldName(string fieldName);
        Task Delete(string id);
    }
}
