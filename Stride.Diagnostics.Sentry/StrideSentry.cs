using System;
using System.Diagnostics;
using Sentry;
using Stride.Core.Settings;

namespace Stride.Diagnostics.Sentry
{
    public static class StrideSentry
    {
        private const string EndpointNotConfigured = "Sentry API endpoint is not configured. Exiting...";
        public static IDisposable Initialize() => Initialize(null);
        public static IDisposable Initialize(Action<SentryOptions> configureOptions)
        {
            var sentryEndpoint = AppSettingsManager.Settings.GetSettings<SentryConfig>();
            
            if (sentryEndpoint == null)
            {
                Debug.WriteLine(EndpointNotConfigured);
                Console.Error.WriteLine(EndpointNotConfigured);
                Environment.Exit(1);
            }

            return SentrySdk.Init(o => 
            {
                o.Dsn = sentryEndpoint.Endpoint;
                o.AddInAppExclude("Stride.");
                configureOptions?.Invoke(o);
            });
        }

        public static IDisposable WithScope(Action<Scope> configureScope)
        {
            var scope = SentrySdk.PushScope();
            SentrySdk.ConfigureScope(configureScope);
            return scope;
        }
    }
}