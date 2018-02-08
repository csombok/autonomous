using Autonomous.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameTry.Public
{
    enum GameObjectType
    {
        Player,
        Car,
        Roadblock,
        Pedestrian
    }

    class GameObjectState
    {
        public float VY { get; }
        public float VX { get; }
        public RectangleF BoundingBox { get; }
        public GameObjectType GameObjectType { get; }
        public string Id { get; }

        // TODO Damage, score
    }

    class GameState
    {
        public IEnumerable<GameObjectState> GameObjectStates { get; }
        public bool Stopped { get; }
    }

    public class PlayerCommand
    {
        public bool MoveLeft { get; }
        public bool MoveRight { get; }

        public bool Acceleration { get; }
    }

    interface IPlayer
    {
        string TeamName { get; }

        // void StartGameLoop(string playerId, IGameStateProvider gameStateProvider, IPlayerCommand command);
        void Initialize(string playerId);
        PlayerCommand Update(GameState gameState);
        void Finish();
    }

    class Player : IPlayer
    {
        public string TeamName => "Team name";

        public void Finish()
        {
            throw new NotImplementedException();
        }

        public void Initialize(string playerId)
        {
            throw new NotImplementedException();
        }

        //public void StartGameLoop(string playerId, IGameStateProvider gameStateProvider, IPlayerCommand command)
        //{
        //    while (gameStateProvider.Stopped)
        //    {
        //        var states = gameStateProvider.GameObjectStates;

        //        command.DoCommand(false, false, 1.0f);
        //    }

        //}

        public PlayerCommand Update(GameState gameState)
        {
            throw new NotImplementedException();
        }
    }
}
