﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTry.GameObjects
{
    public class Barrier : GameObject
    {
        private float rotation;
        private readonly float scale;

        public Barrier(Model model_, float x, float y, float rotation = 0, float scale = 0.013f)
        {
            this.Model = model_;
            X = x;
            Y = y;
            this.rotation = rotation;
            this.scale = scale;
        }

        public override void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            var world = Matrix.CreateRotationY(MathHelper.ToRadians(rotation)) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(X, -0f, -Y));

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
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