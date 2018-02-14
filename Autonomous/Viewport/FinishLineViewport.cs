using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameTry.GameObjects;

namespace MonoGameTry
{
    class FinishLineViewport : ViewportWrapper
    {
        private GameObject _gameObject;

        public FinishLineViewport(int x, int y, int width, int height, GameObject gameObject) : base(x, y, width, height)
        {
            _gameObject = gameObject;

            LookAt = new Vector3(99999, 0, 0);
            CameraOrientation = Vector3.UnitY;
            CameraPosition = new Vector3(_gameObject.X-10, 1f, -_gameObject.Y);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(30), (float)width / height, 0.1f, 500f);
        }

        protected override void UpdateCore()
        {
        }
    }
}
