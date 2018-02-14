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

        private IList<GameObject> _gameObjects;

        public IList<GameObject> GameObjects
        {
            get => _gameObjects;
            set
            {
                _gameObjects = value;
                FirstPlayerPosition = value.Last(x => x is Car).Y;
                LastPlayerPosition = value.First(x => x is Car).Y;
            }
        }


        public float FirstPlayerPosition
        {
            get;
            private set;
        }

        public float LastPlayerPosition
        {
            get;
            private set;
        }
    }
    public interface IGameStateProvider
    {
        GameStateInternal GameStateInternal { get; }
    }
}
