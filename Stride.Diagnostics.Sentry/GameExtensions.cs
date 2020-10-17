using System;
using Sentry;
using Sentry.Protocol;
using Stride.Core.Diagnostics;
using Stride.Games;

namespace Stride.Diagnostics.Sentry
{
    public static class GameExtensions
    {
        public static void UseSentry(this GameBase game)
        {
            game.Activated += Game_Activated;
            game.Deactivated += Game_Deactivated;
            game.Exiting += Game_Exiting;
            game.UnhandledException += Game_UnhandledException;
            game.WindowCreated += Game_WindowCreated;
            SentrySdk.ConfigureScope(s =>
            {
                s.AddEventProcessor(new StrideEventProcessor(game));
            });
            GlobalLogger.GlobalMessageLogged += Logger_GlobalMessageLogged;
        }

        private static void Logger_GlobalMessageLogged(ILogMessage msg)
        {
            if (!msg.IsAtLeast(LogMessageType.Info))
                return;

            SentrySdk.AddBreadcrumb(msg.Text, msg.Module, level: LogLevel(msg.Type));
        }

        private static BreadcrumbLevel LogLevel(LogMessageType type)
        {
            switch (type)
            {
                case LogMessageType.Debug:
                case LogMessageType.Verbose: return BreadcrumbLevel.Debug;
                case LogMessageType.Info: return BreadcrumbLevel.Info;
                case LogMessageType.Warning: return BreadcrumbLevel.Warning;
                case LogMessageType.Error: return BreadcrumbLevel.Error;
                case LogMessageType.Fatal: return BreadcrumbLevel.Critical;
                default: throw new NotSupportedException();
            }
        }

        private static void Game_UnhandledException(object sender, Stride.Games.GameUnhandledExceptionEventArgs ex)
        {
            // Is this needed? This code already exists inside Sentry's SDK on AppDomain.UnhandledException
            if (ex.ExceptionObject is Exception e)
            {
                e.Data["Sentry:Handled"] = false;
                e.Data["Sentry:Mechanism"] = "Game.UnhandledException";
                SentrySdk.CaptureException(e);
                SentrySdk.FlushAsync(TimeSpan.FromSeconds(10)).Wait();
            }

            if (ex.IsTerminating)
            {
                SentrySdk.Close(); // Flush events and close.
            }
        }

        private static void Game_WindowCreated(object sender, System.EventArgs e) => SentrySdk.AddBreadcrumb("Game Window Created", "app.lifecycle");
        private static void Game_Exiting(object sender, System.EventArgs e) => SentrySdk.AddBreadcrumb("Game Exiting", "app.lifecycle");
        private static void Game_Activated(object sender, System.EventArgs e) => SentrySdk.AddBreadcrumb("Game Activated", "app.lifecycle");
        private static void Game_Deactivated(object sender, System.EventArgs e) => SentrySdk.AddBreadcrumb("Game Deactivated", "app.lifecycle");
    }
}
