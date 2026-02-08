using AspStudio.Common;
using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.Threading.Tasks;

namespace API_DVETS.Services
{
    public class MinioService
    {
        private GVar oGvar = new GVar();
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;

        public MinioService()
        {
            _bucketName = oGvar.bucketName;

            var endpoint = oGvar.endpoint.Replace("http://", "").Replace("https://", "");
            bool useSSL = oGvar.endpoint.StartsWith("https://");

            _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(oGvar.username, oGvar.password)
                .WithSSL(useSSL)
                .Build();
        }


        public async Task<string> UploadFileAsync(IFormFile file, string folderPath = "uploads")
        {
            try
            {
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
                }

                string newFileName = $"{Guid.NewGuid()}.webp";
                string objectPath = $"{folderPath}/{newFileName}";

                using var inputStream = file.OpenReadStream();
                using var image = await Image.LoadAsync(inputStream);
                using var outputStream = new MemoryStream();
                await image.SaveAsWebpAsync(outputStream, new WebpEncoder { Quality = 80 });

                outputStream.Seek(0, SeekOrigin.Begin);

                await _minioClient.PutObjectAsync(new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectPath)
                    .WithStreamData(outputStream)
                    .WithObjectSize(outputStream.Length)
                    .WithContentType("image/webp"));

                return $"{oGvar.returnPath}/{_bucketName}/{objectPath}";
            }
            catch (MinioException e)
            {
                throw new Exception($"File upload failed: {e.Message}");
            }
        }

        public async Task DeleteFileByUrlAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL is required");

            var uri = new Uri(url);
            var segments = uri.AbsolutePath.Trim('/').Split('/');
            if (segments.Length < 2)
                throw new Exception("Invalid MinIO URL format");

            string bucket = segments[0];
            string objectName = string.Join('/', segments.Skip(1));

            bool exists = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket));
            if (!exists)
                throw new Exception($"Bucket '{bucket}' not found");

            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName));
        }

    }
}
