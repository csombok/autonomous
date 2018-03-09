using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Autonomous.Public;

namespace Autonomous.SamplePlayer
{
    [Export(typeof(IPlayer))]
    [ExportMetadata("PlayerName", "Example")]
    public class SamplePlayer : IPlayer
    {
        private string _playerId;

        public void Finish()
        {
        }

        public void Initialize(string playerId)
        {
            _playerId = playerId;
        }

        public PlayerAction Update(GameState gameState)
        {
            var self = gameState.GameObjectStates.First(o => o.Id == _playerId);
            var gameObjects = gameState.GameObjectStates.Where(g => g.GameObjectType != GameObjectType.Player).ToList();

            var objectInFront = GetClosestObjectInFront(gameObjects, self);

            float accelerationY = 1;
            bool left = false, right = false;

            float desiredX = GameConstants.LaneWidth / 2;

            if (objectInFront != null)
            {
                if (self.WouldCrashWith(objectInFront))
                    accelerationY = -1;
            }

            float centerX = (self.BoundingBox.Left + self.BoundingBox.Right) / 2;

            if (Math.Abs(desiredX - centerX) > 0.2)
            {
                if (desiredX < centerX)
                    left = true;
                else
                    right = true;
            }
                
            return new PlayerAction() { MoveLeft = left, MoveRight = right, Acceleration = accelerationY };
        }

        private GameObjectState GetClosestObjectInFront(IEnumerable<GameObjectState> objects, GameObjectState self)
        {
            return objects
                          .Where(o => o.BoundingBox.Y > self.BoundingBox.Y)
                          .FirstOrDefault(o => IsOverlappingHorizontally(self, o));
        }

        private bool IsOverlappingHorizontally(GameObjectState self, GameObjectState other)
        {
            var r1 = self.BoundingBox;
            var r2 = other.BoundingBox;
            return Between(r1.Left, r1.Right, r2.Left) ||
                   Between(r1.Left, r1.Right, r2.Right) ||
                   Between(r2.Left, r2.Right, r1.Right) ||
                   Between(r2.Left, r2.Right, r1.Left);
        }

        private static bool Between(float limit1, float limit2, float value)
        {
            return (value >= limit1 && value <= limit2) || (value >= limit2 && value <= limit1);
        }

    }
}
