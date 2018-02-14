using Autonomous.Public;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTry.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameTry
{
    class Dashboard
    {
        private SpriteFont font;
        private SpriteBatch spriteBatch;
        private List<Color> _colors;

        public Dashboard()
        {
            _colors = new List<Color>
            {   Color.White,
                Color.Orange,
                Color.LightBlue,
                Color.LightGreen
            };
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Score"); // Use the name of your sprite font file here instead of 'Score'.
        }

        public void DrawPlayerScores(GraphicsDevice graphics, IEnumerable<Car> players)
        {
            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(graphics);

            spriteBatch.Begin();
            int i = 0;
            foreach (var player in players)
            {
                spriteBatch.DrawString(font, 
                    GetPlayerDashboardText(player),
                    new Vector2(10, i * 20),
                    GetColorByIndex(i));
                i++;
            }

            spriteBatch.End();

            graphics.BlendState = BlendState.Opaque;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            graphics.DepthStencilState = DepthStencilState.Default;

        }

        private Color GetColorByIndex(int index)
        {
            return _colors[Math.Min(index, _colors.Count - 1)];
        }

        private static string GetPlayerDashboardText(Car player)
        {
            return $"{player.PlayerName}: {Math.Round(player.Y)}m Speed:" +
                $" {Math.Round(player.VY * 4)}km/h" +
                $" Damage: {100 - Math.Round(player.Damage * 100)}%";
        }

    }
}
