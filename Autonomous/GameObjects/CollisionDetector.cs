using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameTry.GameObjects
{
    static class CollisionDetector
    {
        public static bool IsCollision(GameObject first, GameObject second)
        {
            var rect1 = GetRectangle(first);
            var rect2 = GetRectangle(second);

            return rect1.Intersects(rect2);
        }

        private static Rectangle GetRectangle(GameObject gameObject)
        {
            float x = gameObject.X - gameObject.Width / 2;
            float y = gameObject.Y - gameObject.Height / 2;

            return new Rectangle((int)x, (int)y, (int)gameObject.Width, (int)gameObject.Height);
        }
    }
}
