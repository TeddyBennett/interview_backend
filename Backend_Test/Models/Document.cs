using System;
using System.ComponentModel.DataAnnotations;

namespace Backend_Test.Models
{
    public class Document
    {
        public string Id { get; set; } // IMM-ddMMyyyy-uuid
        [Required]
        public int DocumentTypeId { get; set; }
        [Required]
        public string DocumentNumber { get; set; }
        [Required]
        public int IssuedCountryId { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class DocumentCreateRequest
    {
        [Required]
        public int DocumentTypeId { get; set; }
        [Required]
        public string DocumentNumber { get; set; }
        [Required]
        public int IssuedCountryId { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
