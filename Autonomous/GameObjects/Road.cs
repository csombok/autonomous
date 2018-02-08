using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTry.GameObjects
{
    class Road : GameObject
    {
        private static Texture2D _texture;
        private static BasicEffect _quadEffect;
        private static Quad _quad;

        private const float Width = GameConstants.RoadWidth;
        private const float QoadHeight = Width;
        public Road()
        {
        }

        public static void LoadContent(ContentManager content, GraphicsDeviceManager graphics)
        {
            _texture = content.Load<Texture2D>("road_t");

            _quadEffect = new BasicEffect(graphics.GraphicsDevice);
            _quadEffect.EnableDefaultLighting();
            _quadEffect.TextureEnabled = true;
            _quadEffect.Texture = _texture;
            _quadEffect.EnableDefaultLighting();

            _quad = new Quad(Vector3.Zero, Vector3.Up, Vector3.Backward, Width, QoadHeight);

        }

        public override void Initialize()
        {
        }

        public override void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            for (int i = 0; i < 100; i++)
            {
                var world = Matrix.CreateTranslation(new Vector3(0, 0, -i * QoadHeight));
                DrawQuad(world, view, projection, device);
            }
        }

        public override void Update(TimeSpan elapsed)
        {
            
        }

        private void DrawQuad(Matrix cworld, Matrix view, Matrix projection, GraphicsDevice device)
        {
            _quadEffect.World = cworld;
            _quadEffect.View = view;
            _quadEffect.Projection = projection;
            _quadEffect.FogEnabled = true;
            _quadEffect.FogStart = 10;
            _quadEffect.FogEnd = 100;
            foreach (EffectPass pass in _quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                        PrimitiveType.TriangleList,
                        _quad.Vertices, 0, 4,
                        _quad.Indices, 0, 2);
            }
        }


    }
}
