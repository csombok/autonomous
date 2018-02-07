using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTry.GameObjects
{
    public class Barrier : GameObject
    {
        private float rotation;
        private readonly float scale;

        public Barrier(Model model, float x, float y)
            : base(model, x, y, 0)
        {
            Width = 0.5f;
        }


        public override void Update(TimeSpan elapsed)
        {
        }
    }
}
