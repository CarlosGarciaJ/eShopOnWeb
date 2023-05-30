using System;
using System.Text;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class OrderItemsReserverServiceBus
    {
        private readonly ILogger _logger;

        public OrderItemsReserverServiceBus(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<OrderItemsReserverServiceBus>();
        }

        [Function("OrderItemsReserverServiceBus")]
        [ExponentialBackoffRetry(3, "00:00:04", "00:10:00")]
        public async Task RunAsync([ServiceBusTrigger("orderitems", Connection = "eshopweb_SERVICEBUS")] string myQueueItem)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            string Connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string containerName = Environment.GetEnvironmentVariable("ContainerName");

            var blobClient = new BlobContainerClient(Connection, containerName);

            var blobName = Guid.NewGuid().ToString() + ".json";
            var blob = blobClient.GetBlobClient(blobName);

            await blob.UploadAsync(GetStream(myQueueItem));
        }

        private Stream GetStream(string queueItem)
        {
            // convert string to stream
            byte[] byteArray = Encoding.ASCII.GetBytes( queueItem );
            MemoryStream stream = new MemoryStream( byteArray ); 
            return stream;
        }
    }
}
