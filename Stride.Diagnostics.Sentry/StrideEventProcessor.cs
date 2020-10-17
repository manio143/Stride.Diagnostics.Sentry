using System.Reflection;
using System.Runtime.InteropServices;
using Sentry;
using Sentry.Extensibility;
using Stride.Games;
using Stride.Graphics;

namespace Stride.Diagnostics.Sentry
{
    internal sealed class StrideEventProcessor : ISentryEventProcessor
    {
        private GameBase game;

        public StrideEventProcessor(GameBase game)
        {
            this.game = game;
        }

        public SentryEvent Process(SentryEvent @event)
        {
            @event.Contexts["launch parameters"] = game.LaunchParameters;
            @event.SetTag("graphicsPlatform", $"{GraphicsDevice.Platform}");
            @event.SetTag("processArch", $"{RuntimeInformation.ProcessArchitecture}");
            @event.Release = GetVersion();
            return @event;
        }

        private string GetVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var fullVersionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var versionAttr = assembly.GetCustomAttribute<AssemblyVersionAttribute>();

            var strideAssembly = typeof(GameBase).Assembly;
            var strideVersion = strideAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

            var version = fullVersionAttr?.InformationalVersion ?? versionAttr?.Version;
            return $"{version} (Stride: {strideVersion})";
        }
    }
}