namespace Autonomous.Impl.Scoring
{
    internal class ScoreFormatter
    {
        public string GetFormattedScore(PlayerScore score)
        {
            if (score == null) return null;

            return score.Stopped
                ? $"{score.Position}. {score.PlayerName}: STOPPED"
                : $"{score.Position}. {score.PlayerName}:" +
                   $" {score.Distance}m " +
                   $" Damage: {score.DamageInPercent}%";
        }

        public string GetFormattedTotalScore(PlayerTotalScore score)
        {
            return score == null ? string.Empty: $"{score.Position}. {score.PlayerName}: {score.TotalScore}";
        }
    }
}
