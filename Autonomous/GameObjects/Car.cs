using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Autonomous.Public;
using Microsoft.Xna.Framework;

namespace MonoGameTry.GameObjects
{
    public class Car : GameObject
    {
        private readonly GameStateManager _gameStateManager;
        private TimeSpan _lastCollision = new TimeSpan();
        public string PlayerName { get; private set; }
        private float _damage = 1.0f;

        public float Damage => _damage;

        public Car(Model model, string playerId, string playerName, GameStateManager gameStateManager, float x = 0)
        {
            _gameStateManager = gameStateManager;
            Model = model;
            Width = 1.7f;
            ModelRotate = 180;
            X = x;
            MaxVY = GameConstants.PlayerMaxSpeed;
            Id = playerId;
            Type = GameObjectType.Player;
            PlayerName = playerName;
        }

        public override void Update(TimeSpan elapsed)
        {
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
            base.Update(elapsed);
        }

        public override void HandleCollision(GameObject other, GameTime gameTime)
        {
            base.HandleCollision(other, gameTime);
            if ((gameTime.TotalGameTime - _lastCollision).TotalMilliseconds > 2000)
            {
                _damage = Math.Max((_damage * 0.9f), 0.5f);
                this.MaxVY = GameConstants.PlayerMaxSpeed * _damage;
            }

            _lastCollision = gameTime.TotalGameTime;
        }
    }
}
