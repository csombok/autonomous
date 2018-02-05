using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameTry.GameObjects;

namespace MonoGameTry
{
    public class GameState
    {
        public IEnumerable<GameObject> GameObjects { get; set; }
    }
    public interface IGameStateProvider
    {
        GameState GameState { get; }
    }
}
