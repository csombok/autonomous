using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTry.Public
{
    public enum GameObjectType
    {
        Player,
        Car,
        Roadblock,
        Pedestrian
    }

    public class GameObjectState
    {
        public GameObjectState(string id, GameObjectType type, RectangleF boundingBox, float vx, float vy)
        {
            Id = id;
            GameObjectType = type;
            BoundingBox = boundingBox;
            VX = vx;
            VY = vy;
        }
        public float VY { get; private set; }
        public float VX { get; private set; }
        public RectangleF BoundingBox { get; private set; }
        public GameObjectType GameObjectType { get; private set; }
        public string Id { get; private set; }

        // TODO Damage, score
    }

    public class GameState
    {
        public GameState(IEnumerable<GameObjectState> gameObjectStates, bool stopped)
        {
            GameObjectStates = gameObjectStates;
            Stopped = stopped;
        }
        public IEnumerable<GameObjectState> GameObjectStates { get; private set; }
        public bool Stopped { get; private set; }
    }

    public class PlayerCommand
    {
        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }

        public float Acceleration { get; set; }
    }

    public interface IPlayer
    {
        string TeamName { get; }

        // void StartGameLoop(string playerId, IGameStateProvider gameStateProvider, IPlayerCommand command);
        void Initialize(string playerId);
        PlayerCommand Update(GameState gameState);
        void Finish();
    }

    public class HumanPlayer : IPlayer
    {
        public string TeamName => "Human";

        public void Finish()
        {
        }

        public void Initialize(string playerId)
        {
        }

        public PlayerCommand Update(GameState gameState)
        {
            float accelerationY = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                accelerationY = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                accelerationY = -1;

            bool left = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool right = Keyboard.GetState().IsKeyDown(Keys.Right);

            return new PlayerCommand() {MoveLeft = left, MoveRight = right, Acceleration = accelerationY};
        }
    }
}
