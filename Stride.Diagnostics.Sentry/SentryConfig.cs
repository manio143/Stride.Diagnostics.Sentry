using Stride.Core;

namespace Stride.Diagnostics.Sentry
{
    [DataContract("SentryConfig")]
    public class SentryConfig
    {
        /// <summary>
        /// Sentry API endpoint to report to.
        /// </summary>
        public string Endpoint { get; set; }
    }
}