using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTry.Strategies;

namespace MonoGameTry.GameObjects
{
    public class CarAgent : GameObject
    {
        private IGameStateProvider _gameStateProvider;
        private IControlStrategy _strategy;

        public CarAgent(Model model, float modelRotate, float width, bool opposite, IGameStateProvider gameStateProvider, IControlStrategy drivingStrategy)
        {
            _gameStateProvider = gameStateProvider;
            Model = model;
            ModelRotate = modelRotate;
            Width = width;
            OppositeDirection = opposite;
            _strategy = drivingStrategy;
            _strategy.GameObject = this;
        }

        public override void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            var carWorld = this.TransformModelToWorld();
            DrawModel(Model, carWorld, view, projection);
            DrawQuad(carWorld, view, projection, device, Color.Green);
        }

        public override void Update(TimeSpan elapsed)
        {
            var state = _strategy.Calculate();
            AccelerationY = state.Acceleration > 0
                ? GameConstants.PlayerAcceleration * state.Acceleration
                : GameConstants.PlayerDeceleration * state.Acceleration;

            VX = 0;

            base.Update(elapsed);
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
