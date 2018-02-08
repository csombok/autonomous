using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameTry.Public;

namespace MonoGameTry
{
    class GameStateManager
    {
        private ConcurrentDictionary<string, PlayerCommand> playerCommands = new ConcurrentDictionary<string, PlayerCommand>();

        public void SetPlayerCommand(string playerId, PlayerCommand command)
        {
            playerCommands.AddOrUpdate(playerId, command, (tmp1, tmp2) => command);
        }

        public PlayerCommand GetPlayerCommand(string playerId)
        {
            PlayerCommand command;
            playerCommands.TryGetValue(playerId, out command);
            return command ?? new PlayerCommand();
        }

        public GameState GameState { get; set; }
    }
}
