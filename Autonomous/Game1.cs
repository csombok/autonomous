using System.Collections.Generic;
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
        private Model model;

        private Texture2D texture;

        private List<ViewportWrapper> viewports = new List<ViewportWrapper>();
        private Car player;
        private Road road;
        private Texture2D metal;


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
            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            model = Content.Load<Model>("Low-Poly-Racing-Car");
            metal = Content.Load<Texture2D>("red_metal");
            Road.LoadContent(Content, graphics);

            player = new Car(model, metal);

            int numViewports = 3;
            int width = graphics.PreferredBackBufferWidth / numViewports;
            int height = graphics.PreferredBackBufferHeight;
            int x = 0;

            viewports.Add(new GameObjectViewport(x, 0, width, height, player));
            x += width;
            viewports.Add(new GameObjectViewport(x, 0, width, height, player));
            x += width;
            viewports.Add(new BirdsEyeViewport(x, 0, width, height));

            road = new Road();

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
            road.Update(gameTime.ElapsedGameTime);

            viewports.ForEach(vp => vp.Update());
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

            foreach (var viewport in viewports)
            {
                graphics.GraphicsDevice.Viewport = viewport.Viewport;
                Draw(gameTime, viewport.View, viewport.Projection);
            }

            graphics.GraphicsDevice.Viewport = original;

            base.Draw(gameTime);
        }

        private void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            road.Draw(gameTime.ElapsedGameTime, view, projection, GraphicsDevice);
            player.Draw(gameTime.ElapsedGameTime, view, projection, GraphicsDevice);
        }
            
    }
}
