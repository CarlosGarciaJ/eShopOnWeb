using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public class DeliveryOrderProcessor
    {
        private readonly ILogger _logger;

        public DeliveryOrderProcessor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DeliveryOrderProcessor>();
        }

        [Function("DeliveryOrderProcessor")]
        [CosmosDBOutput("eshopOnWeb", "OrderItems", Connection = "CosmosDBConnection", CreateIfNotExists = true, PartitionKey = "/buyerId")]
        public async Task<object> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            Console.WriteLine("obteniendo orderItem json...........");

            var orderItem = await new StreamReader(req.Body).ReadToEndAsync();        

            return new { buyerId = "1", orderId = 2, finalPrice = 1000 };
        }
    }
}
