using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameTry.GameObjects;

namespace MonoGameTry
{
    public class GameStateInternal
    {
        public bool Stopped { get; set; }
        public IEnumerable<GameObject> GameObjects { get; set; }
    }
    public interface IGameStateProvider
    {
        GameStateInternal GameStateInternal { get; }
    }
}
