using System;
using Sentry;
using Stride.Diagnostics.Sentry;
using Stride.Engine;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StrideSentry.Initialize())
            using(var game = new Game())
            {
                game.UseSentry();
                game.Run();
            }
        }
    }
}
