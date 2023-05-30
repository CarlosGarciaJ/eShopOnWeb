using System.Net;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class OrderItemsReserver
    {
        private readonly ILogger _logger;

        public OrderItemsReserver(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<OrderItemsReserver>();
        }

        [Function("OrderItemsReserver")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");

            var blobClient = new BlobContainerClient(Connection, containerName);

            var blobName = Guid.NewGuid().ToString() + ".json";
            var blob = blobClient.GetBlobClient(blobName);
            await blob.UploadAsync(req.Body);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Order item created successfuly!");

            return response;
        }
    }
}
