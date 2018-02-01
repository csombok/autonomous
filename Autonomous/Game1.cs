using System.Net.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTry.GameObjects;

namespace MonoGameTry
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Model model;

        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 20), new Vector3(0, 0, 0), Vector3.UnitY);
        private Matrix viewFromTop = Matrix.CreateLookAt(new Vector3(0, 4, -6), new Vector3(0, 0, -6), -Vector3.UnitZ);
        private Matrix projectionFromTop = Matrix.CreateOrthographic(3, 15, 0.1f, 500f);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 500f);

        private Texture2D texture;
        private Effect effect;

        private Viewport viewPort1;
        private Viewport viewPortFromTop;
        BasicEffect quadEffect;

        private Quad quad;
        private Car player;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            viewPort1 = new Viewport();
            viewPort1.X = 0;
            viewPort1.Y = 0;
            viewPort1.Width = 600;
            viewPort1.Height = 480;
            viewPort1.MinDepth = 0;
            viewPort1.MaxDepth = 1;

            viewPortFromTop = new Viewport();
            viewPortFromTop.X = 600;
            viewPortFromTop.Y = 0;
            viewPortFromTop.Width = 200;
            viewPortFromTop.Height = 480;
            viewPortFromTop.MinDepth = 0;
            viewPortFromTop.MaxDepth = 1;
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        VertexDeclaration quadVertexDecl;
        private VertexDeclaration vertexDeclaration;
        private Texture2D metal;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("Low-Poly-Racing-Car");
            texture = Content.Load<Texture2D>("road_t");
            metal = Content.Load<Texture2D>("red_metal");
            player = new Car(model, metal);
            // effect = Content.Load<Effect>("Effects/Ambient");
            // TODO: use this.Content to load your game content here

            quadEffect = new BasicEffect(graphics.GraphicsDevice);
            quadEffect.EnableDefaultLighting();

            //quadEffect.World = Matrix.Identity;
            //quadEffect.View = View;
            //quadEffect.Projection = Projection;
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;
            quadEffect.EnableDefaultLighting();

            quad = new Quad(Vector3.Zero, Vector3.Up, Vector3.Backward, 1 , 2);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime.ElapsedGameTime);
            view = Matrix.CreateLookAt(new Vector3(player.X, 0.2f, -player.Y+1), new Vector3(0, 0, -99999), Vector3.UnitY);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Viewport original = graphics.GraphicsDevice.Viewport;
            graphics.GraphicsDevice.Viewport = viewPort1;
            Draw(gameTime, view, projection);

            graphics.GraphicsDevice.Viewport = viewPortFromTop;
            Draw(gameTime, viewFromTop, projectionFromTop);

            graphics.GraphicsDevice.Viewport = original;

            base.Draw(gameTime);
        }

        private void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {


            for (int i = 0; i < 100; i++)
            {
                world = Matrix.CreateTranslation(new Vector3(0, 0, -i * 2));
                // DrawModel(model, world, view, projection);
                DrawQuad(world, view, projection);
            }

            player.Draw(gameTime.ElapsedGameTime, view, projection);

        }

        private void DrawQuad(Matrix cworld, Matrix view, Matrix projection)
        {
            // graphics.GraphicsDevice.VertexDeclaration = quadVertexDecl;
            quadEffect.World = cworld;
            quadEffect.View = view;
            quadEffect.Projection = projection;
            quadEffect.FogEnabled = true;
            quadEffect.FogStart = 0;
            quadEffect.FogEnd = 10;
            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                        PrimitiveType.TriangleList,
                        quad.Vertices, 0, 4,
                        quad.Indices, 0, 2);
            }
        }
            
    }
}
