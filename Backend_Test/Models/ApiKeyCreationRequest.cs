using System;
using System.ComponentModel.DataAnnotations;

namespace Backend_Test.Models
{
    public class ApiKeyCreationRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string ClientName { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
