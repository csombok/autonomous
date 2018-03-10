using System;
using Autonomous.Impl.GameObjects;
using Microsoft.Xna.Framework;

namespace Autonomous.Impl.Collision
{
    class BorderCollisionStrategy : ICollisionStrategy
    {
        public void HandleCollision(GameObject self, GameObject other, GameTime gameTime)
        {
            if (!(self is Car player)) throw new InvalidOperationException("Strategy is not applicaplbe for non player objects.");
            player.VY = player.VY / 2;
            LastCollision = gameTime.TotalGameTime;
        }

        public TimeSpan LastCollision { get; private set; }
    }
}