using Backend_Test.Helpers;
using Backend_Test.Models;
using Backend_Test.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Backend_Test.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<string> CreateDocumentAsync(DocumentCreateRequest request)
        {
            var document = new Document
            {
                Id = DocumentNumberGenerator.Generate(),
                DocumentTypeId = request.DocumentTypeId,
                DocumentNumber = request.DocumentNumber,
                IssuedCountryId = request.IssuedCountryId,
                IssueDate = request.IssueDate,
                ExpiryDate = request.ExpiryDate,
                CreatedAt = DateTime.UtcNow
            };

            return await _documentRepository.CreateAsync(document);
        }

        public async Task<Document> GetDocumentByIdAsync(string id)
        {
            return await _documentRepository.GetByIdAsync(id);
        }
    }
}
