using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Api.Data;
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
    public static class GetCatalog
    {
        [FunctionName("CatalogItems")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            try
            {
                var contextOptions = new DbContextOptionsBuilder<ColdStartContext>()
                    .UseSqlServer(Environment.GetEnvironmentVariable("AzureSqlDatabase", EnvironmentVariableTarget.Process))
                    .Options;
                var catalog = new Catalog();
                using (var context = new ColdStartContext(contextOptions))
                {
                    catalog.icecreams = await context.Icecreams.ToListAsync();
                }

                return new OkObjectResult(catalog);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new BadRequestObjectResult(ex);
            }
        }
    }
}
