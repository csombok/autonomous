using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameTry
{
#if WINDOWS || LINUX
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
                RunInSilentMode(50);

            else
            {
                using (var game = new Game1())
                {
                    game.Run();
                }
            }
        }

        private static void RunInSilentMode(int fps)
        {
            int timeBetweenUpdatesMs = 1000 / fps;
            using (var game = new Game1())
            {
                game.InitializeModel();
                var sw = Stopwatch.StartNew();
                TimeSpan lastTick = sw.Elapsed;
                while(!game.Stopped)
                {
                    var frameStart = sw.Elapsed;
                    var elapsed = frameStart - lastTick;
                    lastTick = frameStart;
                    game.UpdateModel(new GameTime(frameStart, elapsed));

                    int remaingTimeInFrame = timeBetweenUpdatesMs - (int) (sw.Elapsed -  frameStart).TotalMilliseconds;
                    if (remaingTimeInFrame > 0)
                        Task.Delay(remaingTimeInFrame).Wait();
                }
            }
        }
    }
#endif
}
