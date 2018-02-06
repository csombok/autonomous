using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameTry.GameObjects
{
    public class LigthingEffect
    {
        public LigthingEffect()
        {
        }

        public void Apply(BasicEffect effect)
        {
            effect.LightingEnabled = true;

            effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f);
            effect.DirectionalLight0.Direction = new Vector3(1, -7, -2);
            effect.DirectionalLight0.SpecularColor = new Vector3(0.03f, 0.01f, 0.02f);

            effect.DirectionalLight1.DiffuseColor = new Vector3(0.3f, 0.2f, 0.2f);
            effect.DirectionalLight1.Direction = new Vector3(-2, 1, 100f);
            effect.DirectionalLight1.SpecularColor = new Vector3(0.1f, 0.03f, 0.2f);

            effect.FogStart = 0;
            effect.FogEnd = 100;
        }
    }
}
