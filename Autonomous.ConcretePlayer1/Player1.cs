using System;
using System.ComponentModel.Composition;
using Autonomous.Public;

namespace Autonomous.ConcretePlayer1
{
    [Export(typeof(IPlayer))]
    [ExportMetadata("PlayerName", "Player1")]
    public class Player1 : IPlayer
    {
        public string TeamName => "Player1";

        public void Finish()
        {
            throw new NotImplementedException();
        }

        public void Initialize(string playerId)
        {
            throw new NotImplementedException();
        }

        public PlayerCommand Update(GameState gameState)
        {
            throw new NotImplementedException();
        }
    }
}
