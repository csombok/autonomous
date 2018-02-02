﻿using System;
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
        private readonly Model _model;
        private readonly Texture2D _texture;
        private float acceleration = 0.001f;
        private float VXMax = 0.01f;

        public Car(Model model, Texture2D texture)
        {
            _model = model;
            _texture = texture;            
        }

        public override void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            var carWorld = Matrix.CreateRotationY(MathHelper.ToRadians(25)) * Matrix.CreateScale(0.001f) * Matrix.CreateTranslation(new Vector3(X, -0.01f, - Y));
            DrawModel(_model, carWorld, view, projection, Color.Olive);
        }

        public override void Update(TimeSpan elapsed)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                VY += acceleration;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                VY -= acceleration;

            VX = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                VX -= VXMax;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                VX += VXMax;

            Y += VY;
            X += VX;
        }

        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection, Color color)
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