using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Autonomous.Impl.GameObjects;

namespace Autonomous.Impl.Viewports
{
    class BirdsEyeViewport : ViewportWrapper
    {
        private GameObject _gameObject;

        public BirdsEyeViewport(int x, int y, int width, int height, GameObject gameObject) 
            : base(x, y, width, height, gameObject)
        {
            _gameObject = gameObject;
            float w = 20f;
            Projection = Matrix.CreateOrthographic(w, w*height/width, 0.1f, 500f);
        }

        protected override void UpdateCore()
        {
            CameraPosition = new Vector3(0, 40, -_gameObject.Y-18);
            LookAt = new Vector3(0, 0, -_gameObject.Y-18);
            CameraOrientation = -Vector3.UnitZ;
        }
    }
}
