using BingMapsRESTToolkit;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Point = Microsoft.Azure.Cosmos.Spatial.Point;

namespace Functions
{
    public class UpdateOrderStatus
    {
        [FunctionName("UpdateOrderStatus")]
        public static async Task RunAsync([TimerTrigger("0 0 0 */1 * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            var cosmosConnectionstring = Environment.GetEnvironmentVariable("CosmosDBConnection", EnvironmentVariableTarget.Process);
            using (CosmosClient cosmosClient = new CosmosClient(cosmosConnectionstring))
            {
                Container container = cosmosClient.GetContainer("csBlazorDb", "iceCreamOrders");
                QueryDefinition query = new QueryDefinition("SELECT * FROM iceCreamOrders o where o.Status='Accepted'");

                var orders = new List<CosmosDbOrder>();
                using (FeedIterator<CosmosDbOrder> resultSet = container.GetItemQueryIterator<CosmosDbOrder>(query
                    , requestOptions: new QueryRequestOptions
                    {
                        MaxItemCount = 1
                    }))
                {
                    while (resultSet.HasMoreResults)
                    {
                        FeedResponse<CosmosDbOrder> response = await resultSet.ReadNextAsync();
                        orders.AddRange(response);
                    }
                }

                foreach(var order in orders)
                {
                    order.Status = "Ready";
                    var position = GetLocation(order.FullAddress).Position;
                    order.DeliveryPosition = $"{position.Latitude}, {position.Longitude}";

                    await container.ReplaceItemAsync(order, order.id.ToString());
                }
            }
        }

        private static Point GetLocation(string address)
        {
            var request = new GeocodeRequest()
            {
                Query = address,
                IncludeIso2 = true,
                IncludeNeighborhood = true,
                MaxResults = 5,
                BingMapsKey = Environment.GetEnvironmentVariable("BingMapsApiKey")
            };

            var response = request.Execute().Result;

            if (response != null &&
                response.ResourceSets != null &&
                response.ResourceSets.Length > 0 &&
                response.ResourceSets[0].Resources != null &&
                response.ResourceSets[0].Resources.Length > 0)
            {
                var result = response.ResourceSets[0].Resources[0] as Location;

                return new Point(result.Point.Coordinates[0], result.Point.Coordinates[1]);
            }

            return new Point(0, 0);
        }
    }
}
