using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Api.Data;
using Azure.Storage.Queues;
using ColdStart1App.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Personalizer;
using Microsoft.Azure.CognitiveServices.Personalizer.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API
{
    public static class SendIceCreamOrders
    {
       
        public static string QueueName = "icecreamorders";

        [FunctionName("SendIceCreamOrders")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {

                string requestBody = String.Empty;
                using (StreamReader sReader = new StreamReader(req.Body))
                {
                    requestBody = await sReader.ReadToEndAsync();
                }

                var sendIceCreamOrderRequest = JsonConvert.DeserializeObject<SendIceCreamOrderRequest>(requestBody);

                var contextOptions = new DbContextOptionsBuilder<ColdStartContext>()
                    .UseSqlServer(Environment.GetEnvironmentVariable("AzureSqlDatabase", EnvironmentVariableTarget.Process))
                    .Options;
                using (var context = new ColdStartContext(contextOptions))
                {
                    await context.Orders.AddAsync(sendIceCreamOrderRequest.Preorder);
                    await context.SaveChangesAsync();
                }


                var personalizerEndpoint = Environment.GetEnvironmentVariable("AzurePersonalizerEndpoint", EnvironmentVariableTarget.Process);
                var personalizerKey = Environment.GetEnvironmentVariable("AzurePersonalizerKey", EnvironmentVariableTarget.Process);
                if (!string.IsNullOrWhiteSpace(personalizerEndpoint) || !string.IsNullOrWhiteSpace(personalizerKey))
                {
                    var personalizerClient = new PersonalizerClient(
                        new ApiKeyServiceClientCredentials(personalizerKey))
                    {
                        Endpoint = personalizerEndpoint
                    };

                    var request = new RewardRequest(sendIceCreamOrderRequest.IsRecommended ? 1 : 0);
                    await personalizerClient.RewardAsync(sendIceCreamOrderRequest.EventId, request);      
                }


                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Exception happened when sending: {ex}");
            }

        }
    }
}
