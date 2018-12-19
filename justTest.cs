using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace justTest
{
    public static class justTest
    {
        [FunctionName("justTest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Admin, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string password = req.Query["password"];
            int key = 31;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            password = password ?? data?.password;

            if(password != null){

            StringBuilder inputStringBuild = new StringBuilder(password);  
            StringBuilder outputStringBuild = new StringBuilder(password.Length);  
            char Textch;  
            for (int i = 0; i < password.Length; i++)  
            {  
                Textch = inputStringBuild[i];  
                Textch = (char)(Textch ^ key);  
                outputStringBuild.Append(Textch);  
            }  
            string outputString =  outputStringBuild.ToString();  

            return outputString != null
                ? (ActionResult)new OkObjectResult("Output is " + outputString)
                : new BadRequestObjectResult("Hey Something went wrong. ");

            }

            else{

                return new BadRequestObjectResult("Please input a password to process");
                
            }




        }
    }
}
