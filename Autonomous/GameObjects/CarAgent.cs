using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTry.Strategies;
using Autonomous.Public;

namespace MonoGameTry.GameObjects
{
    public class CarAgent : GameObject
    {
        private IGameStateProvider _gameStateProvider;
        private IControlStrategy _strategy;

        public CarAgent(Model model, float modelRotate, float width, bool opposite, GameObjectType type,  IGameStateProvider gameStateProvider, IControlStrategy drivingStrategy)
        {
            _gameStateProvider = gameStateProvider;
            Model = model;
            ModelRotate = modelRotate;
            Width = width;
            OppositeDirection = opposite;
            if (drivingStrategy != null)
            {
                _strategy = drivingStrategy;
                _strategy.GameObject = this;
            }
            Type = type;
            Id = Guid.NewGuid().ToString();
        }

        public override void Update(TimeSpan elapsed)
        {
            if (_strategy != null)
            {
                var state = _strategy.Calculate();
                AccelerationY = state.Acceleration > 0
                    ? GameConstants.PlayerAcceleration * state.Acceleration
                    : GameConstants.PlayerDeceleration * state.Acceleration;

                VX = state.HorizontalSpeed;
            }

            base.Update(elapsed);
        }
    }
}
