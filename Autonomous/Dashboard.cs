using Autonomous.Public;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("Score"); // Use the name of your sprite font file here instead of 'Score'.
        }

        public void DrawPlayerScores(GraphicsDevice graphics, IEnumerable<IPlayerData> players)
        {
            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(graphics);
            
            spriteBatch.Begin();
            int i = 0;
            foreach (var player in players)
            {
                spriteBatch.DrawString(font, player.PlayerName, new Vector2(20, i * 100), Color.Black);
                i++;
            }

            spriteBatch.End();

            graphics.BlendState = BlendState.Opaque;
            graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            graphics.DepthStencilState = DepthStencilState.Default;

        }
    }
}
