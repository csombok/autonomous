using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTry.GameObjects
{
    public class VanAgent : GameObject
    {
        private float acceleration = 0.001f;
        private float VXMax = 0.01f;

        public VanAgent(Model model, float vy = 0.01f)
        {
            Model = model;
            VY = vy;
            X = 3.5f;
            Width = 2f;
            ModelRotate = 90;
        }

        public override void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            var carWorld = this.TransformModelToWorld();
            DrawModel(Model, carWorld, view, projection);
        }

        public override void Update(TimeSpan elapsed)
        {

            Y += VY;
            X += VX;
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
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
    }
}
