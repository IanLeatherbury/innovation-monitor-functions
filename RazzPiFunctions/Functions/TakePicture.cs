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
    public static class TakePicture
    {
        static ServiceClient serviceClient;
        static string connectionString = "CONNECTION STRING to you device here";

        [FunctionName("TakePicture")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            InvokeMethod().Wait();

            return req.CreateResponse(HttpStatusCode.OK, "TakePicture direct method called");
        }

        private static async Task InvokeMethod()
        {
            var methodInvocation = new CloudToDeviceMethod("TakePicture") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            methodInvocation.SetPayloadJson("'TakePicture has been called!'");

            var response = await serviceClient.InvokeDeviceMethodAsync("DEVICE-NAME goes here", methodInvocation);
        }
    }
}
