using Backend_Test.Models;
using System.Threading.Tasks;

namespace Backend_Test.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        Task<string> CreateAsync(Document document);
        Task<Document> GetByIdAsync(string id);
        Task<bool> UpdateAsync(Document document);
        Task<bool> DeleteAsync(string id);
    }
}
