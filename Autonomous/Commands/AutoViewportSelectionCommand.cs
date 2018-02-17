using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Autonomous.Impl.GameObjects;
using Autonomous.Impl.Viewports;

namespace Autonomous.Impl.Commands
{
    class AutoViewportSelectionCommand : IGameCommand
    {
        private readonly ViewportManager viewportManager;
        private readonly List<Car> players;
        private readonly int humanPlayerIndex;
        private readonly int changeInterval;
        private TimeSpan lastViewportChange = new TimeSpan();
        private int activePlayerIndex;

        public AutoViewportSelectionCommand(ViewportManager viewportManager, List<Car> players, int humanPlayerIndex, int changeInterval = 10000)
        {
            this.viewportManager = viewportManager;
            this.players = players;
            this.humanPlayerIndex = humanPlayerIndex;
            this.changeInterval = changeInterval;
        }

        public void Handle(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - lastViewportChange).TotalMilliseconds > changeInterval)
            {
                var playersInFocus = new List<Car>();
                if (activePlayerIndex == players.Count)
                {
                    playersInFocus = players;
                }
                else
                {
                    if (humanPlayerIndex != -1)
                        playersInFocus.Add(players[humanPlayerIndex]);

                    if (activePlayerIndex != humanPlayerIndex)
                        playersInFocus.Add(players[activePlayerIndex]);
                }

                viewportManager.SetViewports(playersInFocus);

                activePlayerIndex = (++activePlayerIndex) % (players.Count + 1);

                lastViewportChange = gameTime.TotalGameTime;
            }
        }
    }
}
