using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OrderItems;

public static class OrderItemReserverPost
{
    [FunctionName(nameof(OrderItemReserverPost))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
        [Blob("order-items/{rand-guid}.json", FileAccess.Write, Connection = "BlobStorageAccountConnection")] Stream outBlob,
        ILogger log)
    {

        //log.LogInformation("C# HTTP trigger function processed a request.");

        string name = req.Query["name"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        outBlob.Write(Encoding.ASCII.GetBytes(requestBody));
        //dynamic data = JsonConvert.DeserializeObject(requestBody);
        //name = name ?? data?.name;

        string responseMessage = string.IsNullOrEmpty(name)
            ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            : $"Hello, {name}. This HTTP triggered function executed successfully.";

        return new OkObjectResult(responseMessage);
    }
}
