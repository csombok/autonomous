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
    public class Car : GameObject
    {
        private readonly Texture2D _texture;

        public Car(Model model, Texture2D texture)
        {
            Model = model;
            _texture = texture;
            Width = 1.7f;
            ModelRotate = 25;
            MaxVY = GameConstants.PlayerMaxSpeed;
        }

        public override void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            var carWorld = this.TransformModelToWorld();
            DrawModel(Model, carWorld, view, projection, device);
            DrawQuad(carWorld, view, projection, device, Color.Gray);
        }

        public override void Update(TimeSpan elapsed)
        {
            AccelerationY = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                AccelerationY = GameConstants.PlayerAcceleration;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                AccelerationY = -GameConstants.PlayerDeceleration;

            VX = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                VX -= GameConstants.PlayerHoriztontalSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                VX += GameConstants.PlayerHoriztontalSpeed;

            base.Update(elapsed);
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection, GraphicsDevice device)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                if (mesh.Name == "platform")
                    continue;
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;

                    effect.PreferPerPixelLighting = true;

                    if (mesh.Name == "body")
                    {
                        effect.TextureEnabled = true;
                        effect.Texture = _texture;
                    }
                }

                mesh.Draw();
            }
        }
    }
}
