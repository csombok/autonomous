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
    public class BusAgent : GameObject
    {
        private readonly Model _model;
        private float acceleration = 0.001f;
        private float VXMax = 0.01f;

        public BusAgent(Model model, float vy = 0.01f)
        {
            _model = model;
            VY = vy;
            X = 0.32f;
            Y = 1.0f;
        }

        public override void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            var carWorld = Matrix.CreateRotationY(MathHelper.ToRadians(180)) * Matrix.CreateScale(0.03f) * Matrix.CreateTranslation(new Vector3(X, -0.01f, - Y));
            DrawModel(_model, carWorld, view, projection);
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
