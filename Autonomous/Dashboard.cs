using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Autonomous.Impl.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Autonomous.Impl
{
    class Dashboard
    {
        private SpriteFont fontSmall;
        private SpriteFont fontMedium;
        private SpriteFont fontLarge;
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
            fontSmall = content.Load<SpriteFont>("fonts/FontDashboard.small");
            fontMedium = content.Load<SpriteFont>("fonts/FontDashboard.medium");
            fontLarge = content.Load<SpriteFont>("fonts/FontDashboard.large");
        }

        public void DrawStart(GraphicsDevice graphics)
        {
            DrawText(graphics, "START");
        }
        
        public void DrawEnd(GraphicsDevice graphics)
        {
            DrawText(graphics, "FINISH");
        }

        public void DrawText(GraphicsDevice graphics, string text)
        {
            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(graphics);

            spriteBatch.Begin();

            spriteBatch.DrawString(fontLarge, text, new Vector2(graphics.Viewport.Width / 2 - 50, graphics.Viewport.Height / 2), Color.White);
            spriteBatch.End();

            graphics.BlendState = BlendState.Opaque;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            graphics.DepthStencilState = DepthStencilState.Default;
        }

        public void DrawStatus(GraphicsDevice graphics, GameObject playerObject, int playerIndex)
        {
            var player = playerObject as Car;

            if (player == null) return;

            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(graphics);
                   
            spriteBatch.Begin();

            string text = $"Camera: {player.PlayerName}";

            var color = player.Stopped ? Color.Red : GetColorByIndex(playerIndex);
            spriteBatch.DrawString(fontMedium, text, new Vector2(graphics.Viewport.Width / 2 - 50, 10), color);
            spriteBatch.DrawString(fontMedium, text, new Vector2(graphics.Viewport.Width / 2 - 49, 11), Color.Black);
            spriteBatch.End();

            graphics.BlendState = BlendState.Opaque;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            graphics.DepthStencilState = DepthStencilState.Default;

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
            DrawRectangle(graphics, 400, players.Count() * 25 + 25, 0, 0);

            int i = 0;
            foreach (var player in players)
            {
                var color = player.Stopped ? Color.Red : GetColorByIndex(i);
                spriteBatch.DrawString(fontSmall,
                    GetPlayerDashboardText(player),
                    new Vector2(10, 5 + i * 20),
                    color);

                i++;
            }

            spriteBatch.DrawString(fontSmall, fps, new Vector2(10, 5 + players.Count() * 20), Color.Green);

        }

        private Texture2D rect;
        private Vector2 coor;
        private void DrawRectangle(GraphicsDevice graphics, int width, int height, int x, int y)
        {
            if (rect == null)
            {
                rect = new Texture2D(graphics, width, height);
                Color[] data = new Color[width * height];

                coor = new Vector2(x, y);

                for (int j = 0; j < data.Length; ++j)
                    data[j] = Color.FromNonPremultiplied(0, 0, 0, 100);

                rect.SetData(data);
            }
            spriteBatch.Draw(rect, coor, Color.Black);

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
