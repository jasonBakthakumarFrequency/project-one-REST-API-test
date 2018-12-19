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
using System.Security.Cryptography;

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
            string action = req.Query["action"];
            string key = "ilovecandyandyou";

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            password = password ?? data?.password;
            action = action ?? data?.action;

            if(password != null && action != null){

                if(action.Equals("enc")){

                //Encrypt the key    
                byte[] newInputArray = UTF8Encoding.UTF8.GetBytes(password);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();  
                tripleDES.KeySize = 128;
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);  
                tripleDES.Mode = CipherMode.ECB;  
                tripleDES.Padding = PaddingMode.PKCS7;  
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();  
                byte[] resultArray = cTransform.TransformFinalBlock(newInputArray, 0, newInputArray.Length);  
                tripleDES.Clear();  
                string encryptedPassword =  Convert.ToBase64String(resultArray, 0, resultArray.Length);  
                return (ActionResult) new OkObjectResult("Encrypted Password : " + encryptedPassword);

                }
                else{
                    //Decrypt the key
                byte[] inputArray = Convert.FromBase64String(password);  
                TripleDESCryptoServiceProvider newDES = new TripleDESCryptoServiceProvider();  
                newDES.KeySize = 128;
                newDES.Key = UTF8Encoding.UTF8.GetBytes(key);  
                newDES.Mode = CipherMode.ECB;  
                newDES.Padding = PaddingMode.PKCS7;  
                try{

                ICryptoTransform cTransform = newDES.CreateDecryptor();  
                byte[] outputArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);  
                newDES.Clear();   
                string decryptedPassword =  UTF8Encoding.UTF8.GetString(outputArray);  
                return (ActionResult) new OkObjectResult("Decrypted Password : " + decryptedPassword);

                } catch (Exception){

                    return new BadRequestObjectResult("The input is not encrypted data");

                }

                }
              

            }
            
            else{
                //Just send a sorry message that  password and action were not recieved. 

                return new BadRequestObjectResult("Please input a password to process");
                
            }



                /**
                public static string Encrypt(string input, string key)  
        {  
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);  
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();  
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);  
            tripleDES.Mode = CipherMode.ECB;  
            tripleDES.Padding = PaddingMode.PKCS7;  
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();  
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);  
            tripleDES.Clear();  
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);  
        }  
        public static string Decrypt(string input, string key)  
        {  
            byte[] inputArray = Convert.FromBase64String(input);  
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();  
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);  
            tripleDES.Mode = CipherMode.ECB;  
            tripleDES.Padding = PaddingMode.PKCS7;  
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();  
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);  
            tripleDES.Clear();   
            return UTF8Encoding.UTF8.GetString(resultArray);  
        }  
                
                
                
                
                
                
                
                
                 */

            /* StringBuilder inputStringBuild = new StringBuilder(password);  
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
            */





        }
    }
}
