using System.Collections.Generic;

namespace Autonomous.Public
{
    public class GameState
    {
        public IEnumerable<GameObjectState> GameObjectStates { get; }
        public bool Stopped { get; }
    }
}
