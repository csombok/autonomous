using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Autonomous.GameObjects;
using Autonomous.Viewports;

namespace Autonomous
{
    class ViewportFactory
    {
        private readonly GraphicsDeviceManager graphics;

        public ViewportFactory(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public IEnumerable<ViewportWrapper> CreateViewPorts(IEnumerable<GameObject> objects)
        {
            var numViewPorts = objects.Count();
            if (numViewPorts < 1)
                throw new InvalidOperationException("There is no player");

            if (numViewPorts == 1)
            {
                int width = (int)graphics.PreferredBackBufferWidth;
                int height = (int) graphics.PreferredBackBufferHeight;
                yield return CreateViewPort(objects.ElementAt(0), 0, 0, width, height);
            }

            if (numViewPorts == 2)
            {
                int width = (int)graphics.PreferredBackBufferWidth;
                int height = (int)graphics.PreferredBackBufferHeight /2;
                yield return CreateViewPort(objects.ElementAt(0), 0, 0, width, height);
                yield return CreateViewPort(objects.ElementAt(1), 0, height, width, height);
            }

            if (numViewPorts >= 3)
            {
                int width = (int)graphics.PreferredBackBufferWidth / 2;
                int height = (int)graphics.PreferredBackBufferHeight / 2;
                yield return CreateViewPort(objects.ElementAt(0), 0, 0, width, height);
                yield return CreateViewPort(objects.ElementAt(1), 0, height, width, height);
                yield return CreateViewPort(objects.ElementAt(2), width, 0, width, height);
                if (objects.Count()>3)
                    yield return CreateViewPort(objects.ElementAt(3), width, height, width, height);
            }

        }

        static ViewportWrapper CreateViewPort(GameObject gameObject, int x, int y, int width, int height)
        {
            if (gameObject is FinishLine)
                return new FinishLineViewport(x, y, width, height, gameObject);
            return new GameObjectViewport(x, y, width, height, gameObject);
        }
    }
}
