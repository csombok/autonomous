using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autonomous.Public;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTry.GameObjects
{
    public class GameObject
    {
        protected LigthingEffect _defaultLigthing = new LigthingEffect();

        public float X { get; set; }
        public float Y { get; set; }
        public float VY { get; set; }
        public float VX { get; set; }

        public float MaxVY { get; set; }

        public float Width { get; protected set; }

        public string Id { get; protected set; }

        public GameObjectType Type { get; protected set; }

        public RectangleF BoundingBox
        {
            get
            {
                return new RectangleF(X - Width / 2, Y - Height / 2, Width, Height);
            }
        }
        public bool OppositeDirection { get; protected set; }


        public float AccelerationY { get; protected set; }

        protected BoundingBox boundingBox { get; set; }

        public float Height { get; private set; }

        public Model Model { get; protected set; }

        protected float ModelRotate { get; set; }

        public GameObject(Model model, float x, float y, float rotate)
        {
            Model = model;
            X = x;
            Y = y;
            ModelRotate = rotate;
        }

        public GameObject() : this(null, 0, 0, 0)
        {
            MaxVY = 50f / 3.6f;
            VX = 0;
            VY = 0;
        }

        public virtual void Update(TimeSpan elapsed)
        {
            float elapsedSeconds = (float)elapsed.TotalMilliseconds / 1000f;
            VY += AccelerationY * elapsedSeconds;
            //if (VY < 0)
            //    VY = 0;
            if (VY > MaxVY)
                VY = MaxVY;

            if (!OppositeDirection)
            {
                Y += VY * elapsedSeconds;
                X += VX * elapsedSeconds;
            }
            else
            {
                Y -= VY * elapsedSeconds;
                X -= VX * elapsedSeconds;
            }
        }

        public virtual void Draw(TimeSpan elapsed, Matrix view, Matrix projection, GraphicsDevice device)
        {
            var world = TransformModelToWorld();
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    _defaultLigthing.Apply(effect);
                }

                mesh.Draw();
            }
        }

        public virtual void Initialize()
        {
            boundingBox = GetBounds();
            float scaleX = Width / (boundingBox.Max.X - boundingBox.Min.X);
            Height = (boundingBox.Max.Z - boundingBox.Min.Z) * scaleX;
        }

        protected virtual Matrix TransformModelToWorld()
        {
            float scaleX = Width / (boundingBox.Max.X - boundingBox.Min.X);
            float translateZ = (boundingBox.Min.Y);
            float translateX = (boundingBox.Max.X + boundingBox.Min.X) / 2;
            float translateY = (boundingBox.Max.Z + boundingBox.Min.Z) / 2;

            float speedRatio = VY / 10f;
            float turnRotation = Math.Abs(speedRatio - 0) < 0.001f 
                ? 0 
                : VX * 2 / speedRatio;

            float rotate = OppositeDirection ? 180 : 0;
            var worldToView =
                Matrix.CreateRotationY(MathHelper.ToRadians(ModelRotate - turnRotation)) *
                Matrix.CreateTranslation(-translateX, -translateZ, translateY) *
                Matrix.CreateRotationY(MathHelper.ToRadians(rotate)) *
                Matrix.CreateScale(scaleX) *
                Matrix.CreateTranslation(new Vector3(X, 0, -Y));
            return worldToView;
        }

        protected void DrawQuad(Matrix cworld, Matrix view, Matrix projection, GraphicsDevice device, Color color)
        {
            BasicEffect _quadEffect;
            _quadEffect = new BasicEffect(device);
            // _quadEffect.World = cworld;
            _quadEffect.View = view;
            _quadEffect.Projection = projection;
            _quadEffect.DiffuseColor = color.ToVector3();
            _quadEffect.Alpha = 0.9f;

            Quad _quad = new Quad(new Vector3(X, 0.01f, -Y), Vector3.Up, Vector3.Backward, Width, Height);
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
        public BoundingBox GetBounds()
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (ModelMesh mesh in this.Model.Meshes)
            {
                if (this.GetType() == typeof(Car))
                    System.Diagnostics.Debug.WriteLine(mesh.Name);

                if (mesh.Name == "platform")
                    continue;

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    int vertexDataSize = vertexBufferSize / sizeof(float);
                    float[] vertexData = new float[vertexDataSize];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    for (int i = 0; i < vertexDataSize; i += vertexStride / sizeof(float))
                    {
                        Vector3 vertex = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);

                        vertex = Vector3.Transform(vertex, Matrix.CreateRotationY(MathHelper.ToRadians(ModelRotate)));
                        min = Vector3.Min(min, vertex);
                        max = Vector3.Max(max, vertex);
                    }
                }
            }

            return new BoundingBox(min, max);
        }
    }
}
