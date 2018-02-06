using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameTry.GameObjects;

namespace MonoGameTry.Strategies
{
    internal class LaneInfo
    {
        public bool IsInLane { get; set; }
        public bool IsInBrakeZone { get; set; }
        public bool IsInCrashZone { get; set; }
        public bool IsBackCarInCrashZone { get; set; }
        public float CenterX;
    }

    class OvertakingStrategy : IControlStrategy
    {
        private IGameStateProvider _gameStateProvider;
        private float _desiredSpeed;
        private int _preferredLane;

        public OvertakingStrategy(float desiredSpeed, int preferredLane, IGameStateProvider gameStateProvider)
        {
            _desiredSpeed = desiredSpeed;
            _gameStateProvider = gameStateProvider;
            _preferredLane = preferredLane;
        }

        public GameObject GameObject { get; set; }
        public ControlState Calculate()
        {
            if (GameObject == null)
                throw new InvalidOperationException("GameObject not set");


            LaneInfo[] laneInfos = new LaneInfo[2] {GetLaneInfo(0), GetLaneInfo(1)};
            int targetLane = GetTargetLane(laneInfos);

            return GetControlState(targetLane, laneInfos);
        }

        private ControlState GetControlState(int targetLane, LaneInfo[] lanes)
        {
            float targetSpeed = targetLane == _preferredLane ? _desiredSpeed : _desiredSpeed * 1.1f;
            bool brake = lanes[targetLane].IsInBrakeZone || (lanes[0].IsInLane && lanes[0].IsInCrashZone) || (lanes[1].IsInLane && lanes[1].IsInCrashZone);
            float acceleration = 0.0f;
            if (brake || GameObject.VY > targetSpeed)
                acceleration = -1f;
            else if (Math.Abs(GameObject.VY - targetSpeed) < 2)
                acceleration = 0f;
            else
                acceleration = 1f;

            float vx = 0;
            if (Math.Abs(lanes[targetLane].CenterX - GameObject.X) < 0.2f)
                vx = 0;
            else if (lanes[targetLane].CenterX < GameObject.X)
                vx = -1;
            else
                vx = 1;

            if (GameObject.OppositeDirection)
                vx = -vx;

            return new ControlState() {Acceleration = acceleration, HorizontalSpeed = vx};
        }

        private LaneInfo GetLaneInfo(int laneIndex)      
        {
            float width = GameConstants.LaneWidth;
            float centerX = laneIndex == 0 ? width * 1.5f : width / 2;

            if (GameObject.OppositeDirection)
                centerX = -centerX;

            var carFront = GetClosestObjectInLaneFront(_gameStateProvider.GameState.GameObjects, laneIndex);
            var carBack = GetClosestObjectInLaneBack(_gameStateProvider.GameState.GameObjects, laneIndex);

            float selfDistancveToStop = CalculateDistanceToStop(GameObject.VY, GameConstants.PlayerDeceleration);
            bool frontBrake = false;
            bool frontCrash = false;
            if (carFront != null)
            {
                float otherSpeed = carFront.OppositeDirection == GameObject.OppositeDirection ? carFront.VY : 0; //  -go.VY;
                float distanceToStop = CalculateDistanceToStop(otherSpeed, GameConstants.PlayerDeceleration);

                float distanceBetweenCars = Math.Abs(GameObject.Y - carFront.Y) - GameObject.Height / 2 - carFront.Height / 2;

                if (distanceBetweenCars < selfDistancveToStop - distanceToStop)
                    frontCrash = true;

                float plusSafeDistance = 20f + GameObject.VY-carFront.VY;
                if (distanceBetweenCars < selfDistancveToStop - distanceToStop + plusSafeDistance)
                    frontBrake = true;
            }

            bool backCrash = false;
            if (carBack != null)
            {
                float otherSpeed = carBack.OppositeDirection == GameObject.OppositeDirection
                    ? Math.Max(carBack.VY, 130f / 3.6f)
                    : 0; //  -go.VY;

                if (Math.Abs(GameObject.Y - carBack.Y) < GameObject.Height/2 + carBack.Height /2 + 1)
                    backCrash = true;
                else if (otherSpeed < GameObject.VY)
                    backCrash = false;
                else
                { 

                    
                    float distanceToStop = CalculateDistanceToStop(otherSpeed-GameObject.VY, GameConstants.PlayerDeceleration);

                    float distanceBetweenCars =
                        Math.Abs(GameObject.Y - carBack.Y) - GameObject.Height / 2 - carBack.Height / 2;

                    if (distanceBetweenCars < distanceToStop)
                        backCrash = true;
                }
            }

            return new LaneInfo()
            {
                CenterX = centerX,
                IsBackCarInCrashZone = backCrash,
                IsInBrakeZone = frontBrake,
                IsInCrashZone = frontCrash,
                IsInLane = IsInLane(laneIndex, GameObject)
            };
        }

        private int GetTargetLane(LaneInfo[] laneInfos)
        {
            var preferredLaneInfo = laneInfos[_preferredLane];
            var otherLaneInfo = laneInfos[1 - _preferredLane];
            var otherLane = 1 - _preferredLane;

            if (!preferredLaneInfo.IsInBrakeZone && preferredLaneInfo.IsInLane)
                return _preferredLane;

            if ((otherLaneInfo.IsBackCarInCrashZone || otherLaneInfo.IsInCrashZone) && !otherLaneInfo.IsInLane)
                return _preferredLane;

            if ((preferredLaneInfo.IsBackCarInCrashZone || preferredLaneInfo.IsInCrashZone) && !preferredLaneInfo.IsInLane)
                return otherLane;

            if (!preferredLaneInfo.IsInBrakeZone)
                return _preferredLane;

            if (!otherLaneInfo.IsInBrakeZone)
                return otherLane;

            if (otherLaneInfo.IsInCrashZone)
                return _preferredLane;

            if (preferredLaneInfo.IsInCrashZone)
                return otherLane;

            if (preferredLaneInfo.IsInBrakeZone && otherLaneInfo.IsInBrakeZone)
                return _preferredLane;

            throw  new InvalidOperationException("State is not handled");

        }

        private float CalculateDistanceToStop(float v, float breakDeceleration)
        {
            return 0.5f * v * v / breakDeceleration;
        }

        private GameObject GetClosestObjectInLaneFront(IEnumerable<GameObject> objects, int lane)
        {
            if (!GameObject.OppositeDirection)
                return objects.Where(o => o != GameObject && o.Y > GameObject.Y).OrderBy(o => o.Y).FirstOrDefault(o => IsInLane(lane, o));
            return objects.Where(o => o != GameObject && o.Y < GameObject.Y).OrderByDescending(o => o.Y).FirstOrDefault(o => IsInLane(lane, o));
        }

        private GameObject GetClosestObjectInLaneBack(IEnumerable<GameObject> objects, int lane)
        {
            if (!GameObject.OppositeDirection)
                return objects.Where(o => o != GameObject && o.Y < GameObject.Y).OrderByDescending(o => o.Y).FirstOrDefault(o => IsInLane(lane, o));
            return objects.Where(o => o != GameObject && o.Y > GameObject.Y).OrderBy(o => o.Y).FirstOrDefault(o => IsInLane(lane, o));
        }

        private bool IsInLane(int laneIndex, GameObject second)
        {
            float min = laneIndex == 0 ? GameConstants.LaneWidth : 0;
            float max = laneIndex == 0 ? 2 * GameConstants.LaneWidth : GameConstants.LaneWidth;

            if (GameObject.OppositeDirection)
            {
                max = -max;
                min = -min;
            }

            var r2 = second.BoundingBox;
            return Between(min, max, r2.Left) ||
                   Between(min, max, r2.Right);
        }

        private static bool Between(float limit1, float limit2, float value)
        {
            return (value >= limit1 && value <= limit2) || (value >= limit2 && value <= limit1);
        }
    }
}
