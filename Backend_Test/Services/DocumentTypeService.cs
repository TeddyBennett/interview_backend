using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypeService(IDocumentTypeRepository documentTypeRepository)
        {
            _documentTypeRepository = documentTypeRepository;
        }

        public async Task<ApiResponse<IEnumerable<DocumentType>>> GetAllDocumentTypesAsync()
        {
            var documentTypes = await _documentTypeRepository.GetAllAsync();
            return ApiResponse<IEnumerable<DocumentType>>.Ok(documentTypes);
        }

        public async Task<ApiResponse<DocumentType>> GetDocumentTypeByIdAsync(int id)
        {
            var documentType = await _documentTypeRepository.GetByIdAsync(id);
            if (documentType == null)
            {
                return ApiResponse<DocumentType>.Fail("Document Type not found", 404);
            }
            return ApiResponse<DocumentType>.Ok(documentType);
        }
    }
}
