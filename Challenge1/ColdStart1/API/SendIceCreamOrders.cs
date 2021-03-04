using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace API
{
    public static class SendIceCreamOrders
    {
        [FunctionName("SendIceCreamOrders")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = String.Empty;
                using (StreamReader sReader = new StreamReader(req.Body))
                {
                    var body = await sReader.ReadToEndAsync();
                }

                //Send to AzureQueue Storage
                Console.WriteLine(requestBody);

                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Exception happened when sending: {ex}");
            }

        }
    }
}
