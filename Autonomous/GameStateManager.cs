using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autonomous.Public;

namespace MonoGameTry
{
    class GameStateManager
    {
        private ConcurrentDictionary<string, PlayerAction> playerCommands = new ConcurrentDictionary<string, PlayerAction>();

        public void SetPlayerCommand(string playerId, PlayerAction action)
        {
            playerCommands.AddOrUpdate(playerId, action, (tmp1, tmp2) => action);
        }

        public PlayerAction GetPlayerCommand(string playerId)
        {
            PlayerAction action;
            playerCommands.TryGetValue(playerId, out action);
            return action ?? new PlayerAction();
        }

        public GameState GameState { get; set; }
    }
}
