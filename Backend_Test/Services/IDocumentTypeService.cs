using Backend_Test.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public interface IDocumentTypeService
    {
        Task<ApiResponse<IEnumerable<DocumentType>>> GetAllDocumentTypesAsync();
        Task<ApiResponse<DocumentType>> GetDocumentTypeByIdAsync(int id);
    }
}
