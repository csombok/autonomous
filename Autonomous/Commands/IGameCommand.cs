using Microsoft.Xna.Framework;

namespace Autonomous.Commands
{
    interface IGameCommand
    {
        void Handle(GameTime gameTime);
    }
}
