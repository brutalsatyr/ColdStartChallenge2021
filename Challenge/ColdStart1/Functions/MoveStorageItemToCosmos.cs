using System;
using System.Threading.Tasks;
using Api.Data;
using ColdStart1App.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Functions
{
    public static class MoveStorageItemToCosmos
    {
        [FunctionName("MoveStorageItemToCosmos")]
        public static async Task RunAsync([QueueTrigger("icecreamorders", Connection = "AzureQueueStorage")]string iceCreamOrderQueueItem, 
            [CosmosDB( databaseName: "csBlazorDb",
                collectionName: "iceCreamOrders",
                ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<dynamic> document, ILogger Log)
        {
            var iceCreamOrder = JsonConvert.DeserializeObject<Preorder>(iceCreamOrderQueueItem);
            iceCreamOrder.Status = "Accepted";

            CatalogItem iceCream;
            var contextOptions = new DbContextOptionsBuilder<ColdStartContext>()
                   .UseSqlServer(Environment.GetEnvironmentVariable("AzureSqlDatabase", EnvironmentVariableTarget.Process))
                   .Options;
            using (var context = new ColdStartContext(contextOptions))
            {
                iceCream = await context.Icecreams.FirstOrDefaultAsync(x => x.Id == iceCreamOrder.IcecreamId);
            }

            if(iceCream!= null && iceCreamOrder!= null)
            {
                await document.AddAsync(new CosmosDbOrder(iceCreamOrder, iceCream));
            }
            else
            {
                Log.LogError($"Something went wrong when parsing the queueitem: {iceCreamOrderQueueItem}");
            }
        }
    }
}
