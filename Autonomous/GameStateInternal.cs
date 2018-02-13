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
        public IList<GameObject> GameObjects { get; set; }
        public float FirstPlayerPosition
        {
            get { return GameObjects.OfType<Car>().Max(t => t.Y); }
        }

        public float LastPlayerPosition
        {
            get { return GameObjects.OfType<Car>().Min(t => t.Y); }
        }
    }
    public interface IGameStateProvider
    {
        GameStateInternal GameStateInternal { get; }
    }
}
