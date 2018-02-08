using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Autonomous.Public;

namespace MonoGameTry
{
    class PlayerGameLoop
    {
        public static void StartGameLoop(IPlayer player, string playerId, GameStateManager gameStateManager)
        {
            Task task = new Task(() =>
            {
                try
                {
                    player.Initialize(playerId);
                }
                catch (Exception e)
                {
                    // TODO
                }

                bool stopped = false;

                while (!stopped)
                {
                    var state = gameStateManager.GameState;
                    if (state == null)
                        continue;

                    try
                    {
                        var command = player.Update(state);
                        gameStateManager.SetPlayerCommand(playerId, command);
                    }
                    catch (Exception e)
                    {
                        // TODO
                    }

                }
            });

            task.Start();
        }
    }
}
