using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameTry.GameObjects;

namespace MonoGameTry
{
    class ViewportFactory
    {
        public static IEnumerable<ViewportWrapper> CreateViewPorts(IEnumerable<GameObject> objects, float windowWidth, float windowHeight)
        {
            var numViewPorts = objects.Count();
            if (numViewPorts < 1)
                throw new InvalidOperationException("There is no player");

            if (numViewPorts == 1)
            {
                int width = (int)windowWidth;
                int height = (int) windowHeight;
                yield return CreateViewPort(objects.ElementAt(0), 0, 0, width, height);
            }

            if (numViewPorts == 2)
            {
                int width = (int)windowWidth;
                int height = (int)windowHeight/2;
                yield return CreateViewPort(objects.ElementAt(0), 0, 0, width, height);
                yield return CreateViewPort(objects.ElementAt(1), 0, height, width, height);
            }

            if (numViewPorts >= 3)
            {
                int width = (int)windowWidth / 2;
                int height = (int)windowHeight / 2;
                yield return CreateViewPort(objects.ElementAt(0), 0, 0, width, height);
                yield return CreateViewPort(objects.ElementAt(1), 0, height, width, height);
                yield return CreateViewPort(objects.ElementAt(2), width, 0, width, height);
                if (objects.Count()>3)
                    yield return CreateViewPort(objects.ElementAt(3), width, height, width, height);
            }

        }

        static ViewportWrapper CreateViewPort(GameObject gameObject, int x, int y, int width, int height)
        {
              return new GameObjectViewport(x, y, width, height, gameObject);
        }
    }
}
