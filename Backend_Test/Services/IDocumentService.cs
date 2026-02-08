using Backend_Test.Models;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public interface IDocumentService
    {
        Task<string> CreateDocumentAsync(DocumentCreateRequest request);
        Task<Document> GetDocumentByIdAsync(string id);
    }
}
