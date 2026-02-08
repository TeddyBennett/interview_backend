using Backend_Test.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Repositories.Interfaces
{
    public interface IDocumentTypeRepository
    {
        Task<IEnumerable<DocumentType>> GetAllAsync();
        Task<DocumentType> GetByIdAsync(int id);
    }
}
