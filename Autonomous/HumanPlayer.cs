﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Autonomous.Public;

namespace MonoGameTry
{
    public class HumanPlayer : IPlayer
    {
        public string TeamName => "Human";

        public void Finish()
        {
        }

        public void Initialize(string playerId)
        {
        }

        public PlayerCommand Update(GameState gameState)
        {
            float accelerationY = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                accelerationY = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                accelerationY = -1;

            bool left = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool right = Keyboard.GetState().IsKeyDown(Keys.Right);

            return new PlayerCommand() { MoveLeft = left, MoveRight = right, Acceleration = accelerationY };
        }
    }
}
