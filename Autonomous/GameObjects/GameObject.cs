using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameTry.GameObjects
{
    public abstract class GameObject
    {
        public float X { get; protected set; }
        public float Y { get; protected set; }
        public float VY { get; protected set; }
        public float VX { get; protected set; }

        public abstract void Update(TimeSpan elapsed);
        public abstract void Draw(TimeSpan elapsed, Matrix view, Matrix projection);
    }
}
