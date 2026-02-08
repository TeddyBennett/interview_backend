using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Backend_Test.Models
{
    public class PassengerModel
    {
        public int PassengerId { get; set; }
        [Required]
        public string DocId { get; set; } // Reference to Documents.Id
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        public string? FaceImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // This property will be used for incoming file uploads
        public IFormFile? FaceImageFile { get; set; } 

        // Fields for creating both Passenger and Document in one request
        public int? IdfDocTypeId { get; set; }
        public string? IdfDocNumber { get; set; }
        public int? CountryId { get; set; }

        public class PassengerDetail
        {
            public int PassengerId { get; set; }
            public string DocId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Gender { get; set; }
            public string? FaceImageUrl { get; set; }
            public DateTime CreatedAt { get; set; }
            
            // Document details from join
            public string DocumentNumber { get; set; }
            public string DocumentTypeName { get; set; }
            public string CountryName { get; set; }
        }
    }
}