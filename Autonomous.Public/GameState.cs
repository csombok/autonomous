using System.Collections.Generic;

namespace Autonomous.Public
{
    /// <summary>
    /// State of the game. 
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// [INtERNAL] Constructor.
        /// </summary>
        /// <param name="gameObjectStates"></param>
        /// <param name="stopped"></param>
        public GameState(IEnumerable<GameObjectState> gameObjectStates, bool stopped)
        {
            GameObjectStates = gameObjectStates;
            Stopped = stopped;
        }

        /// <summary>
        /// State of all objects the player interacts with.
        /// </summary>
        public IEnumerable<GameObjectState> GameObjectStates { get; private set; }

        /// <summary>
        /// True if the game is finished, otherwise false.
        /// </summary>
        public bool Stopped { get; private set; }
    }
}