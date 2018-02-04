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
        private readonly float rotation;
        private readonly float scale;

        public BuildingA(Model model_, float x, float y, float rotation = 90, float scale = 0.3f)
        {
            this.Model = model_;
            X = x;
            Y = y;
            this.rotation = rotation;
            this.scale = scale;
        }

        public override void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            var world = Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(X, -0.01f, -Y));

            foreach (ModelMesh mesh in Model.Meshes)
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
