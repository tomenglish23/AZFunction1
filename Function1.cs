using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using static System.Net.WebRequestMethods;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;

public static class Function1
{
    /// <summary>
    /// Run
    /// URI: http://localhost:7127/api/Function1?name=Tom
    /// </summary>
    /// <param name="req"></param>
    /// <param name="log"></param>
    /// <returns></returns>
    [FunctionName("Function1")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
    {
        string retVal = "";
        var dt = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");

        log.LogInformation("C# HTTP trigger function processed a request.");

        var host = req.Host.HasValue ? req.Host.Value : "localhost";
        var uri = "";
        if (req.Host.HasValue)
        {
            uri = $"http://{host}/api/Function1"; //?name=Mr%20English";
        }

        string methodType = req.Method;
        string name = req.Query["name"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        name = name ?? data?.name;

        if (!string.IsNullOrEmpty(name))
        {
            retVal = $"Hello {name}, \n"
                    + $"At {dt}, HTTP {methodType} triggered {uri}.\n"
                    + "The request body is null.";
        }
        else
        {
            retVal = $"At {dt}, HTTP {methodType} triggered {uri}.\n"
                    + "You could pass a parameters in the query string or request body.";
        }

        return new OkObjectResult(retVal);
    }
}