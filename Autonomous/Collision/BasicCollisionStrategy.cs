using System;
using Autonomous.Impl.GameObjects;
using Autonomous.Public;
using Microsoft.Xna.Framework;

namespace Autonomous.Impl.Collision
{
    class BasicCollisionStrategy : ICollisionStrategy
    {
        public void HandleCollision(GameObject self, GameObject other, GameTime gameTime)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (IsStaticObject(self)) return;

            if (HasOppositeDirection(self, other) || IsStaticObject(other))
            {
                self.VY = 0;
            }
            else
            {
                var avgSpeed = (self.VY + other.VY) / 2;
                var factor = self.Y <= other.Y ? 0.8f : 1.2f;
                self.VY = avgSpeed * factor;
            }

            LastCollision = gameTime.TotalGameTime;
        }

        public TimeSpan LastCollision { get; private set; }

        private bool IsStaticObject(GameObject gameObject)
        {
            return gameObject.Type == GameObjectType.Roadblock ||
                   gameObject.Type == GameObjectType.BusStop;
        }

        protected bool HasOppositeDirection(GameObject self, GameObject other)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (other == null) throw new ArgumentNullException(nameof(other));

            return self.OppositeDirection != other.OppositeDirection
                   || Math.Abs(Math.Sign(self.VY) - Math.Sign(other.VY)) == 2;
        }
    }
}