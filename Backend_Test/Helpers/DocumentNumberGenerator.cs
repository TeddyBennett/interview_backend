namespace Backend_Test.Helpers
{
    public class DocumentNumberGenerator
    {
        public static string Generate()
        {
            DateTime now = DateTime.Now;
            string dateString = now.ToString("ddMMyyyy");
            string uuid = Guid.NewGuid().ToString().Split('-')[0];

            return $"IMM-{dateString}-{uuid}";
        }
    }
}
