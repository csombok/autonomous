using System.Collections.Generic;
using System.Linq;
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
        private List<GameObject> gameObjects = new List<GameObject>();
        private Car player;
        private Road road;
        private Texture2D metal;

        private BuildingA building;
        private bool collision;


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
            model = Content.Load<Model>("Cars/Porshe/carrgt");

            Road.LoadContent(Content, graphics);

            player = new Car(model);

            road = new Road();          
  
            gameObjects = new List<GameObject>() { road, player };
            gameObjects.AddRange(GenerateBuildings());
            gameObjects.AddRange(GenerateTrees());
            gameObjects.AddRange(GenerateInitialCarAgents());

            gameObjects.ForEach(go => go.Initialize());

            int numViewports = 1;
            int width1 = (int)(graphics.PreferredBackBufferWidth * 0.8f);
            int width2 = (int)(graphics.PreferredBackBufferWidth * 0.2f);
            int height = graphics.PreferredBackBufferHeight;
            int x = 0;

            viewports.Add(new GameObjectViewport(x, 0, width1, height, player));
            x += width1;
            viewports.Add(new BirdsEyeViewport(x, 0, width2, height, player));
        }

        private IEnumerable<Tree> GenerateTrees()
        {
            var model = Content.Load<Model>("Tree\\fir");

            for (int i = 0; i < 200; i++)
            {
                float x = i % 4 == 0 ? 8 : 9;
                float scaleLeft = i % 2 == 0 ? 0.8f : 1.3f;
                float scaleRight = i % 3 == 0 ? 0.9f : 1.3f;
                yield return new Tree(model, x, i * 20f + 10,  scaleLeft);
                yield return new Tree(model, -x, i * 20f + 10,  scaleRight);
            }
        }

        private IEnumerable<BuildingA> GenerateBuildings()
        {
            var buildingModel = Content.Load<Model>("BuildingA");
            for (int i = 0; i < 100; i++)
            {
                float roatationLeft = i % 3 == 0 ? 90 : 180;
                float roatationRight = i % 2 == 0 ? 90 : 180;
                float x = i % 4 == 0 ? 10 : 12;
                float scaleLeft = i % 2 == 0 ? 0.5f : 0.3f;
                float scaleRight = i % 3 == 0 ? 0.3f : 0.6f;
                yield return new BuildingA(buildingModel, x, i * 20f, roatationLeft, scaleLeft);
                yield return new BuildingA(buildingModel, -x, i * 20f, roatationRight, scaleRight);
            }
        }

        private IEnumerable<GameObject> GenerateInitialCarAgents()
        {
            var vanModel = Content.Load<Model>("kendo");
            var busModel = Content.Load<Model>("bus");

            const float laneWidth = GameConstants.LaneWidth;
            const float vanWidth = 2f;
            for (int i = 0; i < 10; i++)
            {
                var van = new CarAgent(vanModel, 90, vanWidth, false)
                {
                    VY = 70f / 3.6f,
                    MaxVY = 120f / 3.6f,
                    X = laneWidth / 2,
                    Y = i * 100
                };
                yield return van;

                van = new CarAgent(vanModel, 90, vanWidth, true)
                {
                    VY = 70f / 3.6f,
                    MaxVY = 120f / 3.6f,
                    X = -laneWidth / 2,
                    Y = i * 200
                };
                yield return van;

                var bus = new CarAgent(busModel, 180f, 2.6f, false)
                {
                    VY = 50f / 3.6f,
                    MaxVY = 100f / 3.6f,
                    X = laneWidth * 1.45f,
                    Y = i * 100 + 20
                };

                yield return bus;

                bus = new CarAgent(busModel, 180f, 2.6f, true)
                {
                    VY = 50f / 3.6f,
                    MaxVY = 100f / 3.6f,
                    X = -laneWidth * 1.45f,
                    Y = i * 200 + 20
                };

                yield return bus;

            }
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

            gameObjects.ForEach(go => go.Update(gameTime.ElapsedGameTime));


            collision = (gameObjects.OfType<CarAgent>().Any(x => CollisionDetector.IsCollision(x, player))) ;
            if (!collision)
            {
                if (player.X - player.Width / 2 < -6)
                    collision = true;
                if (player.X + player.Width / 2 > 6)
                    collision = true;
            }
            viewports.ForEach(vp => vp.Update());
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(collision ? Color.Red : Color.CornflowerBlue);

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
            gameObjects.ForEach(go => go.Draw(gameTime.ElapsedGameTime, view, projection, GraphicsDevice));
        }

    }
}
