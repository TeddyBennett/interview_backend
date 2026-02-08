namespace Backend_Test.Models
{
    public class ErrorLogModel
    {
        public int id { get; set; }
        public DateTime timestamp { get; set; }
        public string status { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public string path { get; set; }
        public string req_from { get; set; }
    }
}
