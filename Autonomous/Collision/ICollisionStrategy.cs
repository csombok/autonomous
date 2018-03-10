using System;
using Autonomous.Impl.GameObjects;
using Microsoft.Xna.Framework;

namespace Autonomous.Impl.Collision
{
    public interface ICollisionStrategy
    {
        void HandleCollision(GameObject self, GameObject other, GameTime gameTime);
        TimeSpan LastCollision { get; }
    }
}
