using System;
using Autonomous.Impl.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Autonomous.Impl.Strategies;
using Autonomous.Public;

namespace Autonomous.Impl.GameObjects
{
    public class CarAgent : GameObject
    {
        private readonly ICollisionStrategy _collisionStrategy;
        private readonly IControlStrategy _strategy;

        public CarAgent(Model model, float modelRotate, float width, float height, bool opposite,
            GameObjectType type, IControlStrategy drivingStrategy, ICollisionStrategy collisionStrategy)
            : base(model, true)
        {
            Width = width;
            HardCodedHeight = height;
            ModelRotate = modelRotate;
            OppositeDirection = opposite;
            if (drivingStrategy != null)
            {
                _strategy = drivingStrategy;
                _strategy.GameObject = this;
            }

            _collisionStrategy = collisionStrategy;

            Type = type;
            Id = Guid.NewGuid().ToString();
        }

        public CarAgent(Model model, float modelRotate, float width, float height, bool opposite,
            GameObjectType type, IControlStrategy drivingStrategy)
            : this(model, modelRotate, width, height, opposite, type, drivingStrategy, new BasicCollisionStrategy())
        { }

        public override void Update(GameTime gameTime)
        {
            if (_strategy != null)
            {
                var state = _strategy.Calculate();
                AccelerationY = state.Acceleration > 0
                    ? GameConstants.PlayerAcceleration * state.Acceleration
                    : GameConstants.PlayerDeceleration * state.Acceleration;

                VX = state.HorizontalSpeed;
            }

            base.Update(gameTime);
        }

        public override void HandleCollision(GameObject other, GameTime gameTime)
        {
            _collisionStrategy.HandleCollision(this, other, gameTime);
        }
    }
}
