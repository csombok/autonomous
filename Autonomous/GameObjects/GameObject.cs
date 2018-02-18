using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autonomous.Public;
using Autonomous.Impl.Utilities;
using Autonomous.Impl.Viewports;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Autonomous.Impl.GameObjects
{
    public class GameObject
    {
        protected LigthingEffect _defaultLigthing = new LigthingEffect();
        private readonly bool agentObject;

        public float X { get; set; }

        public float Y { get; set; }

        public float VY { get; set; }

        public float VX { get; set; }

        public float MaxVY { get; set; }

        public float? MaxX { get; set; }

        public bool Stopped => MaxVY == 0;

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

        public float HardCodedHeight { get; set; }

        public Model Model { get; protected set; }

        protected float ModelRotate { get; set; }

        public GameObject(Model model, bool agentObject)
        {
            Model = model;
            this.agentObject = agentObject;
            MaxVY = 50f / 3.6f;
        }

        public GameObject(bool agentObject) : this(null, agentObject) { }

        public virtual void Update(GameTime gameTime)
        {
            if (!agentObject) return;

            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
            VY += AccelerationY * elapsedSeconds;
            if (VY < 0)
                VY = 0;

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

        protected bool IsInView(ViewportWrapper viewport)
        {
            float cameraY = -viewport.CameraPosition.Z;
            return this.Y > cameraY - 10 && this.Y < cameraY + 400;


        }
        public virtual void Draw(TimeSpan elapsed, ViewportWrapper viewport, GraphicsDevice device)
        {
            if (!IsInView(viewport))
                return;
            var world = TransformModelToWorld();
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = viewport.View;
                    effect.Projection = viewport.Projection;
                    effect.EnableDefaultLighting();
                    _defaultLigthing.Apply(effect);
                }

                mesh.Draw();
            }
        }

        public virtual void Initialize()
        {
            if (Model == null)
            {
                Height = HardCodedHeight;
                return;
            }

            boundingBox = GetBounds();
            float scaleX = Width / (boundingBox.Max.X - boundingBox.Min.X);
            Height = (boundingBox.Max.Z - boundingBox.Min.Z) * scaleX;

            if (Math.Abs(Height - HardCodedHeight) > 0.01 && HardCodedHeight > 0)
                Console.WriteLine($"{Width} - {Height} - {HardCodedHeight}");

        }

        protected virtual Matrix TransformModelToWorld()
        {
            float scaleX = Width / (boundingBox.Max.X - boundingBox.Min.X);
            float translateZ = (boundingBox.Min.Y);
            float translateX = (boundingBox.Max.X + boundingBox.Min.X) / 2;
            float translateY = (boundingBox.Max.Z + boundingBox.Min.Z) / 2;

            float speedRatio = Math.Abs(VY) < 1 ? 0.1f : VY / 10f;
            float turnRotation = Math.Abs(speedRatio - 0) < 0.001f
                ? 0
                : VX * 2 / speedRatio;

            turnRotation = turnRotation.SignedMax(18);
            float turnRotationZ = (turnRotation / 1.4f).SignedMax(4);

            float rotate = OppositeDirection ? 180 : 0;
            var worldToView =
                Matrix.CreateRotationY(MathHelper.ToRadians(ModelRotate - turnRotation)) *
                Matrix.CreateTranslation(-translateX, -translateZ, translateY) *
                Matrix.CreateRotationY(MathHelper.ToRadians(rotate)) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(turnRotationZ)) *
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

        public virtual void HandleCollision(GameObject other, GameTime gameTime)
        {
            if (other == null) return;

            if (this.OppositeDirection == other.OppositeDirection)
            {
                if (this.Y <= other.Y)
                {
                    this.VY = other.VY * 0.2f;
                }
                else
                {
                    this.VY += Math.Max(this.MaxVY, this.VY / 3);
                }
            }
            else
            {
                this.VY = 0;
            }
        }

        
    }
}
