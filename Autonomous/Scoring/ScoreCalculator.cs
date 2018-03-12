﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autonomous.Impl.GameObjects;

namespace Autonomous.Impl.Scoring
{
    internal class ScoreCalculator
    {
        private readonly ScoreCsvImporter _scoreImporter;
        private const float TieGameTrashold = 0.0001f;

        public ScoreCalculator(ScoreCsvImporter scoreImporter)
        {
            _scoreImporter = scoreImporter ?? throw new ArgumentNullException(nameof(scoreImporter));
        }

        public IEnumerable<PlayerScore> GetPlayerScores(IEnumerable<Car> players, TimeSpan timeElapsed)
        {
            if (players == null) throw new ArgumentNullException(nameof(players));

            var sortedPlayers = players
                .OrderBy(car => !car.Stopped)
                .ThenByDescending(car => car.Y);

            int position = 1;
            float previousY = -1;
            int previousScore = 0;
            foreach (var player in sortedPlayers)
            {
                var distance = (int)Math.Round(player.Y);
                var damageInPercent = (int)Math.Round(player.Damage * 100);
                var speed = (int)Math.Round(player.VY * 4);
                var score = 0;

                if (!player.Stopped && position <=3)
                {
                    score = Math.Abs(player.Y - previousY) < TieGameTrashold
                        ? previousScore
                        : (int) Math.Pow(2, 4 - position);
                }

                previousY = player.Y;
                previousScore = score;

                yield return new PlayerScore(player.Id, player.PlayerName, distance, position, 
                    damageInPercent, speed, timeElapsed, player.Stopped, score);

                ++position;
            }
        }

        public IEnumerable<PlayerTotalScore> TotalScores
        {
            get
            {
                return _scoreImporter
                    .Scores
                    .GroupBy(p => p.PlayerName)
                    .Select(g => new PlayerTotalScore(g.Key, g.Sum(c => c.Score)))
                    .OrderByDescending(p => p.TotalScore);
            }
        }
    }
}
