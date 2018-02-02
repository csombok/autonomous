using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameTry.GameObjects;

namespace MonoGameTry
{
    class BirdsEyeViewport : ViewportWrapper
    {

        public BirdsEyeViewport(int x, int y, int width, int height) : base(x, y, width, height)
        {
            Projection = Matrix.CreateOrthographic(3, 15, 0.1f, 500f);
        }

        protected override void UpdateCore()
        {
            CameraPosition = new Vector3(0, 4, -6);
            LookAt = new Vector3(0, 0, -6);
            CameraOrientation = -Vector3.UnitZ;
        }
    }
}
