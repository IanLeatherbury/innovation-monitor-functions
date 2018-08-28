using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazzPiFunctions.Services
{
    public class AnalyticService : IDisposable
    {
        static TelemetryClient _telemetryClient;
        static RequestTelemetry _requestTelemetry;

        private IOperationHolder<RequestTelemetry> _operation;

        public AnalyticService(RequestTelemetry requestTelemetry)
        {
            if (_telemetryClient == null)
            {
                _telemetryClient = new TelemetryClient(TelemetryConfiguration.Active);
                _telemetryClient.InstrumentationKey = Keys.Analytics.Key; // Environment.GetEnvironmentVariable("APP_INSIGHTS_KEY");
            }

            _requestTelemetry = requestTelemetry;

            // start tracking request operation
            _operation = _telemetryClient.StartOperation(_requestTelemetry);
        }

        public void TrackException(Exception e)
        {
            // track exceptions that occur
            _telemetryClient.TrackException(e);
        }

        public void Dispose()
        {
            // stop the operation (and track telemetry).
            _telemetryClient.StopOperation(_operation);
        }
    }

}
