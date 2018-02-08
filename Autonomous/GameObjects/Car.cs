using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Autonomous.Public;

namespace MonoGameTry.GameObjects
{
    internal class Car : GameObject
    {
        private readonly GameStateManager _gameStateManager;

        public Car(Model model, string playerId, GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
            Model = model;
            Width = 1.7f;
            ModelRotate = 180;
            MaxVY = GameConstants.PlayerMaxSpeed;
            Id = playerId;
            Type = GameObjectType.Player;
        }

        public override void Update(TimeSpan elapsed)
        {
            var command = _gameStateManager.GetPlayerCommand(Id);
            AccelerationY = 0;
            
            if (command.Acceleration > 0)
                AccelerationY = Math.Min(command.Acceleration,1) * GameConstants.PlayerAcceleration;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                AccelerationY = Math.Max(command.Acceleration, -1) * GameConstants.PlayerDeceleration;

            VX = 0;
            if (command.MoveLeft)
                VX -= GameConstants.PlayerHoriztontalSpeed;
            if (command.MoveRight)
                VX += GameConstants.PlayerHoriztontalSpeed;
            base.Update(elapsed);
        }
    }
}
