using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Required for IFormFile

namespace Backend_Test.Models
{
    public class PassengerModel
    {
        public int PassengerId { get; set; }
        [Required]
        public int DocId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; } // Changed to nullable DateTime
        [Required]
        public int CountryId { get; set; } // Foreign key to Countries table
        [Required]
        public int IdfDocTypeId { get; set; } // Foreign key to DocumentTypes table
        [Required]
        public string IdfDocNumber { get; set; }
        [Required]
        public string Gender { get; set; }
        public string? FaceImageUrl { get; set; } // Renamed for clarity and MinIO integration
        public DateTime CreatedAt { get; set; } // Added missing CreatedAt property
        
        // This property will be used for incoming file uploads, not stored in DB
        public IFormFile? FaceImageFile { get; set; } 


        public class PassengerDetail // Assuming this is for richer representation, might include joined data
        {
            public int PassengerId { get; set; }
            public int DocId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Gender { get; set; }
            public int CountryId { get; set; }
            public string CountryName { get; set; } // To hold joined country name
            public int IdfDocTypeId { get; set; }
            public string IdfDocTypeName { get; set; } // To hold joined document type name
            public string IdfDocNumber { get; set; }
            public string? FaceImageUrl { get; set; }
            public DateTime CreatedAt { get; set; } // Added missing CreatedAt property
        }
    }
}
