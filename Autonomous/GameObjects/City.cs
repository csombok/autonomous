using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTry.GameObjects
{
    public class City : GameObject
    {
        private float rotation;
        private readonly float scale;

        public City(Model model_, float x, float y, float rotation = 0, float scale = 0.35f)
        {
            this.Model = model_;
            X = x;
            Y = y;
            this.rotation = rotation;
            this.scale = scale;
        }

        public override void Draw(TimeSpan elapsed, ViewportWrapper viewport, GraphicsDevice device)
        {
            if (!IsInView(viewport))
                return;
            var world = Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(X, -16f, -Y));

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

        public override void Update(TimeSpan elapsed)
        {
        }
    }
}
