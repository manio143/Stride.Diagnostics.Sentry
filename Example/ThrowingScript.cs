using Sentry;
using Stride.Core;
using Stride.Diagnostics.Sentry;
using Stride.Engine;

namespace Example
{
    [DataContract("ThrowingScript")]
    public class ThrowingScript : SyncScript
    {
        public override void Update()
        {
            using (StrideSentry.WithScope(s => s.SetTag("script", nameof(ThrowingScript))))
            {
                throw new System.ApplicationException("Encountered an error!");
            }
        }
    }
}