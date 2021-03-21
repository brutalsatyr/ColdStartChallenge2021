using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Api.Data;
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
    public static class GetRecommendation
    {
        [FunctionName("Recommendation")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                string requestBody = String.Empty;
                using (StreamReader sReader = new StreamReader(req.Body))
                {
                    requestBody = await sReader.ReadToEndAsync();
                }
                var recommendationRequest = JsonConvert.DeserializeObject<RecommendationRequest>(requestBody);

                var personalizerEndpoint = Environment.GetEnvironmentVariable("AzurePersonalizerEndpoint", EnvironmentVariableTarget.Process);
                var personalizerKey = Environment.GetEnvironmentVariable("AzurePersonalizerKey", EnvironmentVariableTarget.Process);
                if(!string.IsNullOrWhiteSpace(personalizerEndpoint) || !string.IsNullOrWhiteSpace(personalizerKey))
                {
                    var personalizerClient = new PersonalizerClient(
                        new ApiKeyServiceClientCredentials(personalizerKey))
                    {
                        Endpoint = personalizerEndpoint
                    };

                    var actions = GetRankableActions(recommendationRequest.Catalogitems);
                    var contextFeatures = GetContextFeatures(recommendationRequest.UserName);
                    var excludeActions = new List<string>();
                    var request = new RankRequest(actions, contextFeatures, excludeActions, Guid.NewGuid().ToString());
                    var response = await personalizerClient.RankAsync(request);

                    var recommendationReponse = new RecommendationResponse
                    {
                        eventId = response.EventId,
                        PrefferedItemId = Convert.ToInt32(response.RewardActionId)
                    };

                    return new OkObjectResult(recommendationReponse);
                }
                return new BadRequestObjectResult("No endpoint or key configured");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new BadRequestObjectResult(ex);
            }
        }

        private static IList<RankableAction> GetRankableActions(List<CatalogItem> catalogItems)
        {
            var actions = new List<RankableAction>();
            foreach(var item in catalogItems)
            {
                actions.Add(new RankableAction()
                {
                    Id = item.Id.ToString(),
                    Features = new List<object>
                    {
                        new { name= item.Name},
                        new { description= item.Description}
                    }

                });
            }
            return actions;
        }

        private static IList<object> GetContextFeatures(string userName)
        {
            
            var list = new List<object>()
            {
                new { userName= userName},
                new { timeofDay = DateTime.Now.Hour.ToString() },
                new { dayofWeek= DateTime.Now.Day.ToString()}
            };
            return list;
        }

    }
}
