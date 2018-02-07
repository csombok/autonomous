using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTry.GameObjects
{
    public class BuildingA : GameObject
    {
        public BuildingA(Model model, float x, float y, float rotation = 90, float width = 7f)
            : base(model, x, y, rotation)
        {
            Width = width;
        }

        public override void Update(TimeSpan elapsed)
        {
        }
    }
}
