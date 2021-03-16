using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Api.Data;
using Azure.Storage.Queues;
using ColdStart1App.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

                //var connectionString = Environment.GetEnvironmentVariable("AzureQueueStorage", EnvironmentVariableTarget.Process);
                //var queueClient = new QueueClient(connectionString, QueueName);
                //await queueClient.SendMessageAsync(requestBody);
                ////Send to AzureQueue Storage
                //Console.WriteLine(requestBody);

                var preorder = JsonConvert.DeserializeObject<Preorder>(requestBody);

                var contextOptions = new DbContextOptionsBuilder<ColdStartContext>()
                    .UseSqlServer(Environment.GetEnvironmentVariable("AzureSqlDatabase", EnvironmentVariableTarget.Process))
                    .Options;
                using (var context = new ColdStartContext(contextOptions))
                {
                    await context.Orders.AddAsync(preorder);
                    await context.SaveChangesAsync();
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
