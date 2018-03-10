using System;
using Autonomous.Impl.Collision;
using Microsoft.Xna.Framework.Graphics;
using Autonomous.Public;
using Microsoft.Xna.Framework;

namespace Autonomous.Impl.GameObjects
{
    public class Car : GameObject
    {
        private readonly GameStateManager _gameStateManager;
        private readonly ICollisionStrategy _collisionStrategy;
        public string PlayerName { get; }

        public Car(Model model, string playerId, string playerName, ICollisionStrategy collisionStrategy, GameStateManager gameStateManager, Color color, float x = 0, float y = 0)
            : base(model, true)
        {
            _collisionStrategy = collisionStrategy;
            _gameStateManager = gameStateManager;
            Color = color;
            Model = model;
            Width = GameConstants.PlayerWidth;
            ModelRotate = 180;
            X = x;
            Y = y;
            MaxVY = GameConstants.PlayerMaxSpeed;
            Id = playerId;
            Type = GameObjectType.Player;
            PlayerName = playerName;
            MaxX = 6f;
        }

        public Color Color { get; }
        public float Damage { get; internal set; } = 0f;
        public TimeSpan LastCollision => _collisionStrategy.LastCollision;

        public override void Update(GameTime gameTime)
        {
            if (Stopped) return;

            var command = _gameStateManager.GetPlayerCommand(Id);
            AccelerationY = 0;

            if (command.Acceleration > 0)
                AccelerationY = Math.Min(command.Acceleration, 1) * GameConstants.PlayerAcceleration;
            else
                AccelerationY = Math.Max(command.Acceleration, -1) * GameConstants.PlayerDeceleration;

            VX = 0;
            if (command.MoveLeft)
                VX -= GameConstants.PlayerHoriztontalSpeed;
            if (command.MoveRight)
                VX += GameConstants.PlayerHoriztontalSpeed;

            base.Update(gameTime);

            if (MaxX.HasValue && X - Width / 2 < -MaxX)
            {
                X = -MaxX.Value + Width / 2;
                HandleCollision(null, gameTime);
            }

            if (MaxX.HasValue && X + Width / 2 > MaxX)
            {
                X = MaxX.Value - Width / 2;
                HandleCollision(null, gameTime);
            }
        }

        public override void HandleCollision(GameObject other, GameTime gameTime)
        {
            _collisionStrategy.HandleCollision(this, other, gameTime);
        }
    }
}
