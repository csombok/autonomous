using System;
using System.Collections.Generic;
using System.Linq;
using Autonomous.Impl.GameObjects;
using Autonomous.Public;
using Microsoft.Xna.Framework;

namespace Autonomous.Impl.Collision
{
    class PlayerCollisionStrategy : ICollisionStrategy
    {
        private readonly ICollisionStrategy _borderCollisionStrategy;
        private readonly ICollisionStrategy _basicCollisionStrategy;
        private readonly Dictionary<string, TimeSpan> _collisions = new Dictionary<string, TimeSpan>();

        private const double CollisionPenalityInterval = 1000;
        private const float SpeedLossFactor = 3f;

        public PlayerCollisionStrategy(ICollisionStrategy basicCollisionStrategy, ICollisionStrategy borderCollisionStrategy)
        {
            _borderCollisionStrategy = borderCollisionStrategy ?? throw new ArgumentNullException(nameof(borderCollisionStrategy));
            _basicCollisionStrategy = basicCollisionStrategy ?? throw new ArgumentNullException(nameof(basicCollisionStrategy));
        }

        public void HandleCollision(GameObject self, GameObject other, GameTime gameTime)
        {
            if (!(self is Car player)) throw new InvalidOperationException("Strategy is not applicaplbe for non player objects.");

            // Collision to the course border
            if (other == null)
            {
                _borderCollisionStrategy.HandleCollision(player, null, gameTime);

            }
            else
            {                
                _basicCollisionStrategy?.HandleCollision(player, other, gameTime);
            }

            SetCollisionPenality(player, other, gameTime);
        }

        public TimeSpan LastCollision => _collisions.Values.Any() ? _collisions.Values.Max(t => t) : TimeSpan.MinValue;

        private void SetCollisionPenality(Car player, GameObject other, GameTime gameTime)
        {
            string key = other != null ? other.Id : "border";
            if (!HadCollisionIn(key, gameTime, CollisionPenalityInterval))
            {
                player.Damage = Math.Min((player.Damage + 0.1f), 1f);
                var maxSpeed = GameConstants.PlayerMaxSpeed;
                float speedLoss = (maxSpeed * player.Damage / SpeedLossFactor);
                player.MaxVY = Math.Abs(player.Damage - 1f) < 0.0001f ? 0 : maxSpeed - speedLoss;
                _collisions[key] = gameTime.TotalGameTime;
            }
        }

        private bool HadCollisionIn(string key, GameTime gameTime, double interval)
        {
            if (_collisions.TryGetValue(key, out var lastCollision) &&
                (gameTime.TotalGameTime - lastCollision).TotalMilliseconds < interval)
            {
                return true;
            }

            return false;
        }
    }
}