using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameTry.GameObjects
{
    public class Car : GameObject
    {        
        public Car(Model model)
        {
            Model = model;
            Width = 1.7f;
            ModelRotate = 180;
            MaxVY = GameConstants.PlayerMaxSpeed;
        }

        public override void Update(TimeSpan elapsed)
        {
            AccelerationY = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                AccelerationY = GameConstants.PlayerAcceleration;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                AccelerationY = -GameConstants.PlayerDeceleration;

            VX = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                VX -= GameConstants.PlayerHoriztontalSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                VX += GameConstants.PlayerHoriztontalSpeed;

            base.Update(elapsed);
        }
    }
}
