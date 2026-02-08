using System.ComponentModel.DataAnnotations;

namespace Backend_Test.Models
{
    public class PassengerModel
    {
        public int passenger_id { get; set; }
        [Required]
        public int doc_id { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        public string date_of_birth { get; set; }
        [Required]
        public int country { get; set; }
        [Required]
        public int idf_doc_type { get; set; }
        [Required]
        public string idf_doc_number { get; set; }
        [Required]
        public string gender { get; set; }
        public string? face_image { get; set; }
        public string? face_image_name { get; set; }
        public IFormFile? File { get; set; }



        public class PassengerDetail
        {
            public int passenger_id { get; set; }
            [Required]
            public int doc_id { get; set; }
            [Required]
            public string first_name { get; set; }
            [Required]
            public string last_name { get; set; }
            public string date_of_birth { get; set; }
            [Required]
            public string country { get; set; }
            [Required]
            public string idf_doc_type { get; set; }
            [Required]
            public string idf_doc_number { get; set; }
            [Required]
            public string gender { get; set; }
            public string? face_image { get; set; }
            public string? face_image_name { get; set; }
        }
    }
}
