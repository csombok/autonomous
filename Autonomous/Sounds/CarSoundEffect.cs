using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Autonomous.Impl.Sounds
{
    internal class CarSoundEffect
    {
        private Song _sound;
        private bool _started;

        public void Initialize(ContentManager content)
        {
            if (_sound == null && !_started)
            {
                _sound = content.Load<Song>("Sounds/caracross_16bit");
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(_sound);
                _started = true;
            }
        }
    }
}
