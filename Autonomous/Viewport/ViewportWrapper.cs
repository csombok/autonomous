using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTry
{
    public abstract class ViewportWrapper
    {
        public Vector3 CameraPosition { get; protected set; }

        public Vector3 LookAt { get; protected set; }

        public Vector3 CameraOrientation { get; protected set; }
        public ViewportWrapper(int x, int y, int width, int height)
        {
            Viewport = new Viewport()
            {
                X = x,
                Y = y,
                Width = width,
                Height = height,
                MinDepth = 0,
                MaxDepth = 1
            };            
        }
        public Viewport Viewport { get; private set; }

        public Matrix View { get; private set; }

        public Matrix Projection { get; protected set; }

        protected abstract void UpdateCore();

        public void Update()
        {
            UpdateCore();
            View = Matrix.CreateLookAt(CameraPosition, LookAt, CameraOrientation);            
        }
    }
}
