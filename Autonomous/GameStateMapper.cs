using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autonomous.Public;
using MonoGameTry.GameObjects;

namespace MonoGameTry
{
    class GameStateMapper
    {
        public static GameState GameStateToPublic(GameStateInternal state)
        {
            return new GameState(state.GameObjects.Select(GameObjectStateToPublic).ToList(), state.Stopped);
        }
        private static GameObjectState GameObjectStateToPublic(GameObject gameObject)
        {
            return new GameObjectState(gameObject.Id, gameObject.Type, gameObject.BoundingBox, gameObject.VX,
               gameObject.OppositeDirection ? -gameObject.VY : gameObject.VY);
        }
    }
}
