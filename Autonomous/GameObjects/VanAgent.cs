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
    public class VanAgent : GameObject
    {
        public VanAgent(Model model, float vy = 50f/3.6f)
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
