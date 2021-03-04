using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ColdStart1App.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API
{
    public static class GetCatalog
    {
        [FunctionName("CatalogItems")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous,"get",Route = null)] HttpRequest req, ILogger log)
        {
            var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var catalog = Path.Combine(binDirectory, "../Shared/catalog.json");
            var json = File.ReadAllText(catalog);
            var list= JsonConvert.DeserializeObject<Catalog>(json);

            return new OkObjectResult(list);
        }
    }
}
