﻿using Microsoft.Xna.Framework.Graphics;

namespace Autonomous.Impl.GameObjects
{
    public class Barrier : GameObject
    {
        public Barrier(Model model, float x, float y)
            : base(model, false)
        {
            Width = 0.5f;
        }
    }
}
