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
        private readonly Model model_;

        public BuildingA(Model model_, float x, float y)
        {
            this.model_ = model_;
            X = x;
            Y = y;
        }

        public override void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            var world = Matrix.CreateRotationY(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.03f) * Matrix.CreateTranslation(new Vector3(X, -0.01f, -Y));

            foreach (ModelMesh mesh in model_.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.PreferPerPixelLighting = true;
                }

                mesh.Draw();
            }
        }

        public override void Update(TimeSpan elapsed)
        {
        }
    }
}
