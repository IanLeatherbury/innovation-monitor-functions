using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using RazzPiFunctions.Services;

namespace RazzPiFunctions
{
    public static class GetTempHumidity
    {
        [FunctionName("GetTempHumidity")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function [GetTempHumidity] processed a request.");

            using (var analytic = new AnalyticService(new RequestTelemetry
            {
                Name = nameof(GetTempHumidity)
            }))
            {
                try
                {
                    var kvps = req.GetQueryNameValuePairs();

                    var tempHumidity = CosmosDataService.Instance("TempHumidityCollection").GetTempHumidityModel();

                    if (tempHumidity.Count == 0)
                        return req.CreateErrorResponse(HttpStatusCode.NoContent, "No results found.");

                    return req.CreateResponse(HttpStatusCode.OK, tempHumidity);
                }
                catch (Exception e)
                {
                    // track exceptions that occur
                    analytic.TrackException(e);
                    return req.CreateErrorResponse(HttpStatusCode.BadRequest, e.Message, e);
                }
            }
        }
    }
}
