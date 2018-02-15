using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autonomous.Viewports;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Autonomous.GameObjects
{
    public class City : GameObject
    {
        public City(Model model_, float x, float y): base(model_, false )
        {
            X = x;
            Y = y;
        }

        public override void Draw(TimeSpan elapsed, ViewportWrapper viewport, GraphicsDevice device)
        {
            if (!IsInView(viewport))
                return;
            var world = Matrix.CreateRotationY(MathHelper.ToRadians(0)) * Matrix.CreateScale(0.35f) * Matrix.CreateTranslation(new Vector3(X, -16f, -Y));

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = viewport.View;
                    effect.Projection = viewport.Projection;
                    _defaultLigthing.Apply(effect);
                }

                mesh.Draw();
            }
        }
    }
}
