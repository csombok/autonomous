using System;
using System.Collections.Generic;
using System.IO;

namespace Autonomous.Impl.Scoring
{
    internal class ScoreCsvExporter
    {
        private readonly string _filePath;

        public ScoreCsvExporter(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

            if (!Path.GetExtension(_filePath).ToLower().EndsWith(".csv"))
            {
                throw new ArgumentException($"Wrong CSV file name: {filePath}");
            }
        }

        public void ExportScoresToCsv(IEnumerable<PlayerScore> scores)
        {
            var lines = new List<string>();

            if (!File.Exists(_filePath))
            {
                lines.Add("Position,Name,Distance(m),Damage(%),Time(ms)");
            }

            foreach (var score in scores)
            {
                lines.Add($"{score.Position}," +
                          $"{score.PlayerName}," +
                          $"{score.Distance}," +
                          $"{score.DamageInPercent}," +
                          $"{score.TimeElapsed.TotalMilliseconds}");
            }

            File.AppendAllLines(_filePath, lines);
        }
    }
}
