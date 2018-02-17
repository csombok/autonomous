using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autonomous.Impl;
using Microsoft.Xna.Framework;

namespace Autonomous
{
/// <summary>
/// The main class.
/// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "-quiet")
                RunInSilentMode(1);

            else
            {
                using (var game = new Game1(1000, 0.5f))
                {
                    game.Run();
                }
            }
        }

        private static void RunInSilentMode(float timeAccelerationFactor)
        {
            using (var game = new Game1(10000, 0.2f))
            {
                game.InitializeModel();
                var sw = Stopwatch.StartNew();
                int lastTick = (int)(sw.Elapsed.TotalMilliseconds * timeAccelerationFactor);
                while (!game.Stopped)
                {
                    int frameStart = (int)(sw.Elapsed.TotalMilliseconds * timeAccelerationFactor);
                    var elapsed = frameStart - lastTick;
                    lastTick = frameStart;
                    game.UpdateModel(new GameTime(TimeSpan.FromMilliseconds(frameStart), TimeSpan.FromMilliseconds(elapsed)));
                }
            }
        }
    }
}
