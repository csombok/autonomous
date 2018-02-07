using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTry.GameObjects
{
    public class Tree : GameObject
    {

        public Tree(Model model, float x, float y, float width = 5)
            : base(model, x, y, 0)
        {
            Width = width;
        }

        public override void Update(TimeSpan elapsed)
        {
        }
    }
}
