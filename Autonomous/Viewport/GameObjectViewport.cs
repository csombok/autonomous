using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameTry.GameObjects;

namespace MonoGameTry
{
    class GameObjectViewport : ViewportWrapper
    {
        private GameObject _gameObject;

        public GameObjectViewport(int x, int y, int width, int height, GameObject gameObject) : base(x, y, width, height)
        {
            _gameObject = gameObject;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(30), (float)width / height, 0.1f, 500f);
        }

        protected override void UpdateCore()
        {
            CameraPosition = new Vector3(_gameObject.X, 2f, -_gameObject.Y + 10);
            LookAt = new Vector3(0, 0, -99999);
            CameraOrientation = Vector3.UnitY;
        }
    }
}
