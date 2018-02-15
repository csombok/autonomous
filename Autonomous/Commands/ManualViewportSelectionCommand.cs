using Autonomous.Viewports;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Autonomous.GameObjects;
using System;
using System.Collections.Generic;

namespace Autonomous.Commands
{
    class ManualViewportSelectionCommand : IGameCommand
    {
        private readonly ViewportManager viewportManager;
        private readonly List<Car> players;

        public ManualViewportSelectionCommand(ViewportManager viewportManager, List<Car> players)
        {
            this.viewportManager = viewportManager;
            this.players = players;
        }

        public void Handle(GameTime gameTime)
        {
            try
            {
                if (Keyboard.GetState().IsKeyDown(Keys.F1) && players.Count >= 1)
                    viewportManager.SetViewports(new List<Car> { players[0] });

                if (Keyboard.GetState().IsKeyDown(Keys.F2) && players.Count >= 2)
                    viewportManager.SetViewports(new List<Car> { players[1] });

                if (Keyboard.GetState().IsKeyDown(Keys.F3) && players.Count >= 3)
                    viewportManager.SetViewports(new List<Car> { players[2] });

                if (Keyboard.GetState().IsKeyDown(Keys.F4) && players.Count >= 4)
                    viewportManager.SetViewports(new List<Car> { players[3] });

                if (Keyboard.GetState().IsKeyDown(Keys.F5))
                    viewportManager.SetViewports(players);

            }
            catch (Exception)
            {
            }
        }
    }
}
