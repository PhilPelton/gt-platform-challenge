using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace PlatformChallenge
{
    class WebhookInterface
    {
        public static void SendDataListToWebhook(List<CsvLine> csvData)
        {

            try
            {
                foreach (CsvLine processLine in csvData){
                    SendDataLineToWebhook(CreateJsonString(processLine));
                    Console.WriteLine("Processed ID: " + processLine.Id);
                    System.Threading.Thread.Sleep(Config.timeBetweenWebPosts);
                }
            }
            catch (Exception e)
            {
                Logging.HandleError("sendDataListToWebhook", e);
            }
        }

        private static void SendDataLineToWebhook(string jsonData)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Config.webHookURL);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }
                var response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    //no result in this example, so no need to process.
                }  
            }
            catch(Exception e)
            {
                Logging.HandleError("sendDataLineToWebhook", e);
            }
        }

        private static string CreateJsonString(CsvLine inputLine)
        {
            return JsonConvert.SerializeObject(inputLine);
        }
    }
}
