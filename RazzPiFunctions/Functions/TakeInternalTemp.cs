using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace RazzPiFunctions.Functions
{
    public static class TakeInternalTemp
    {
        static ServiceClient serviceClient;
        static string connectionString = "CONNECTION STRING to your IoT Device";

        [FunctionName("TakeInternalTemp")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function [TakeInternalTemp] processed a request.");

            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            InvokeMethod().Wait();

            return req.CreateResponse(HttpStatusCode.OK, "TakeInternalTemp called");
        }

        private static async Task InvokeMethod()
        {
            var methodInvocation = new CloudToDeviceMethod("TakeInternalTemp") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            methodInvocation.SetPayloadJson("'TakeInternalTemp has been called!'");

            var response = await serviceClient.InvokeDeviceMethodAsync("DEVICE-NAME goes here", methodInvocation);
        }
    }
}
