namespace Autonomous.Impl.Scoring
{
    public class PlayerTotalScore
    {
        public PlayerTotalScore(string playerName, int totalScore)
        {
            PlayerName = playerName;
            TotalScore = totalScore;
        }

        public string PlayerName { get; }
        public int TotalScore { get;  }
        public int Position { get; set; }
    }
}
