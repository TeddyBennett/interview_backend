namespace AspStudio.Common
{
    public class GVar
    {
        public string conn = "";
        public string clientKey = "";
        public string secretKey = "";
        public string issuer = "";
        public string audience = "";

        public string endpoint = "";
        public string username = "";
        public string password = "";
        public string bucketName = "";
        public string returnPath = "";


        public GVar()
        {
            var configuation = GetConfiguration();
            if (configuation.GetSection("ENV").Value == "Prod")
            {
                conn = configuation.GetSection("ConnectionStrings").GetSection("con_prod").Value;
            }
            else if (configuation.GetSection("ENV").Value == "Dev")
            {
                conn = configuation.GetSection("ConnectionStrings").GetSection("con_dev").Value;
            }

            clientKey = configuation.GetSection("Jwt").GetSection("ClientKey").Value;
            secretKey = configuation.GetSection("Jwt").GetSection("Key").Value;
            issuer = configuation.GetSection("Jwt").GetSection("Issuer").Value;
            audience = configuation.GetSection("Jwt").GetSection("Audience").Value;

            endpoint = configuation.GetSection("Minio").GetSection("Endpoint").Value;
            username = configuation.GetSection("Minio").GetSection("Username").Value;
            password = configuation.GetSection("Minio").GetSection("Password").Value;
            bucketName = configuation.GetSection("Minio").GetSection("BucketName").Value;
            returnPath = configuation.GetSection("Minio").GetSection("ReturnPath").Value;


        }


        public IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

    }
}
