using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Newtonsoft.Json;
using RazzPiFunctions.Models;
using RazzPiFunctions.Services;
using System;

namespace RazzPiFunctions.Functions
{
    public static class TempHumidityEventTrigger
    {
        [FunctionName("TempHumidityEventTrigger")]
        public static void Run([EventHubTrigger("razzpi", Connection = "EventHub")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Hub trigger function [TempHumidityEventTrigger] processed a message: {myEventHubMessage}");

            TempHumidityModel data = JsonConvert.DeserializeObject<TempHumidityModel>(myEventHubMessage);
            // JSON looks like: {"deviceId": "Raspberry Pi - Python","externalTemp": 66.058200,"humidity": 77.534756,"internalTemp": 64.736600, "timeOfReading": "2018-08-27 08:21:13.733881"}

            try
            {
                var uploadData = CosmosDataService.Instance("TempHumidityCollection").InsertItemAsync(data);
            }
            catch (Exception e)
            {
                //TODO: Implement analytics service
                Console.WriteLine(e);
            }
        }
    }
}
