using System;

namespace Autonomous.Impl.Scoring
{
    public class PlayerScore
    {
        public PlayerScore(string playerName, int distance, int position, int damageInPercent, int speed, TimeSpan timeElapsed, bool stopped)
        {
            Distance = distance;
            Position = position;
            DamageInPercent = damageInPercent;
            Speed = speed;
            TimeElapsed = timeElapsed;
            Stopped = stopped;
            PlayerName = playerName;
        }
        
        public string PlayerName { get; }
        public int Distance { get; }
        public int Position { get; }
        public int DamageInPercent { get; }
        public int Speed { get; }
        public TimeSpan TimeElapsed { get; }
        public bool Stopped { get; }
    }
}
