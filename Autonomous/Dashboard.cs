using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Autonomous.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autonomous
{
    class Dashboard
    {
        private SpriteFont font;
        private SpriteBatch spriteBatch;
        private List<Color> _colors;

        public Dashboard()
        {
            _colors = new List<Color>
            {   Color.LightCyan,
                Color.Orange,
                Color.LightBlue,
                Color.LightGreen
            };
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Score"); // Use the name of your sprite font file here instead of 'Score'.
        }

        public void Draw(GraphicsDevice graphics, IEnumerable<Car> players, string fps)
        {
            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(graphics);

            spriteBatch.Begin();

            DrawScores(graphics, players, fps);

            spriteBatch.End();

            graphics.BlendState = BlendState.Opaque;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            graphics.DepthStencilState = DepthStencilState.Default;

        }

        private void DrawScores(GraphicsDevice graphics, IEnumerable<Car> players, string fps)
        {
            DrawRectangle(graphics, 400, players.Count() * 25 + 25);

            int i = 0;
            foreach (var player in players)
            {
                var color = player.Stopped ? Color.Red : GetColorByIndex(i);
                spriteBatch.DrawString(font,
                    GetPlayerDashboardText(player),
                    new Vector2(10, 5 + i * 20),
                    color);

                i++;
            }

            spriteBatch.DrawString(font, fps, new Vector2(10, 5 + players.Count() * 20), Color.Green);

        }

        private void DrawRectangle(GraphicsDevice graphics, int width, int height)
        {
            Texture2D rect = new Texture2D(graphics, width, height);
            Color[] data = new Color[width * height];

            Vector2 coor = new Vector2(0, 0);
            spriteBatch.Draw(rect, coor, Color.Black);

            for (int j = 0; j < data.Length; ++j)
                data[j] = Color.FromNonPremultiplied(0, 0, 0, 100);

            rect.SetData(data);
        }

        private Color GetColorByIndex(int index)
        {
            return _colors[Math.Min(index, _colors.Count - 1)];
        }

        private static string GetPlayerDashboardText(Car player)
        {
            var status = player.Stopped
                ? "STOPPED"
                : $"{Math.Round(player.Y)}m Speed: " +
                    $"{Math.Round(player.VY * 4)}km/h" +
                    $" Damage: {Math.Round(player.Damage * 100)}%";

            return $"{player.PlayerName}: {status}";
        }

    }
}
