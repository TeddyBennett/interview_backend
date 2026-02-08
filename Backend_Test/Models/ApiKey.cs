using System;

namespace Backend_Test.Models
{
    public class ApiKey
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string HashedKey { get; set; }
        public bool IsEnabled { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
