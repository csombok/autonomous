using System;
using System.ComponentModel.Composition;
using Autonomous.Public;

namespace Autonomous.ConcretePlayer2
{
    [Export(typeof(IPlayer))]
    [ExportMetadata("PlayerName", "Player2")]
    public class Player2 : IPlayer
    {
        public string TeamName => "Player2";

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
