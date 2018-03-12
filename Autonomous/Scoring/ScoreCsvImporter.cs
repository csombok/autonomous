using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Autonomous.Impl.Scoring
{
    internal class ScoreCsvImporter
    {
        private readonly string _filePath;

        public ScoreCsvImporter(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

            if (!Path.GetExtension(_filePath).ToLower().EndsWith(".csv"))
            {
                throw new ArgumentException($"Wrong CSV file name: {filePath}");
            }
        }

        public IEnumerable<PlayerScore> Scores
        {
            get
            {
                var scores = new List<PlayerScore>();
                if (File.Exists(_filePath))
                {
                    try
                    {
                        var lines = File.ReadAllLines(_filePath);
                        if (lines.Any())
                        {
                            foreach (var line in lines.Skip(1))
                            {
                                var parts = line.Split(',');

                                scores.Add(new PlayerScore(
                                    playerId: null,
                                    playerName: ParseString(parts, 1),
                                    distance: ParseInt(parts, 2),
                                    damageInPercent: ParseInt(parts, 3),
                                    position: ParseInt(parts, 0),
                                    speed: 0,
                                    timeElapsed: new TimeSpan((int)ParseFloat(parts, 4) * 10000),
                                    stopped: false,
                                    score: ParseInt(parts, 5)));
                            }
                        }
                    }
                    catch
                    {
                        // TODO: ignored
                    }
                }
                return scores;
            }

        }


        private static string ParseString(string[] parts, int index)
        {
            return index >= parts.Length ? string.Empty : parts[index];
        }

        private static float ParseFloat(string[] parts, int index)
        {
            if (index >= parts.Length) return default(float);

            return float.TryParse(parts[index], out float value) ? value : default(float);
        }

        private static int ParseInt(string[] parts, int index)
        {
            if (index >= parts.Length) return default(int);

            return int.TryParse(parts[index], out int value) ? value : default(int);
        }

    }
}
