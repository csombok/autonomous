using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameTry.GameObjects;
using MonoGameTry.Strategies;

namespace MonoGameTry
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game, IGameStateProvider
    {
        GraphicsDeviceManager graphics;
        private Model model;

        private List<ViewportWrapper> viewports = new List<ViewportWrapper>();
        private List<GameObject> gameObjects = new List<GameObject>();
        private Car player;
        private Road road;
        private Texture2D metal;
        private AgentFactory _agentFactory;

        private BuildingA building;
        private bool collision;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _agentFactory = new AgentFactory(this);
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

            _agentFactory.LoadContent(Content);

            road = new Road();

            gameObjects = new List<GameObject>() { road, player };
            gameObjects.AddRange(GenerateBuildings());
            gameObjects.AddRange(GenerateTrees());
            gameObjects.AddRange(GenerateBarriers());
            gameObjects.AddRange(GenerateCity());
            gameObjects.AddRange(GenerateTerrain());
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

            float offset = 150;
            for (int i = 0; i < 200; i++)
            {
                float x = i % 4 == 0 ? 8 : 9;
                float scaleLeft = i % 2 == 0 ? 0.8f : 1.3f;
                float scaleRight = i % 3 == 0 ? 0.9f : 1.3f;
                yield return new Tree(model, x, i * 20f + offset, scaleLeft);
                yield return new Tree(model, -x, i * 20f + offset, scaleRight);
            }
        }

        private IEnumerable<Barrier> GenerateBarriers()
        {
            var model = Content.Load<Model>("barrier");
            float offset = 300;
            for (int i = 0; i < 500; i++)
            {
                yield return new Barrier(model, 6.3f, i * 3.3f + offset);
                yield return new Barrier(model, -6.3f, i * 3.3f + offset);
            }
        }

        private IEnumerable<City> GenerateCity()
        {
            var model = Content.Load<Model>("City/The City");
            float offset = 800;
            for (int i = 0; i < 10; i++)
            {
                yield return new City(model, 0f, i * 450 + offset);             
            }
        }

        private IEnumerable<Terrain> GenerateTerrain()
        {
            var model = Content.Load<Model>("mountain/mountains");
            for (int i = 0; i < 50; i++)
            {
                yield return new Terrain(model, 8f, i * 200);
            }
        }

        private IEnumerable<BuildingA> GenerateBuildings()
        {
            var buildingModel = Content.Load<Model>("BuildingA");
            for (int i = 0; i < 100; i++)
            {
                float offset = 500;
                float roatationLeft = i % 3 == 0 ? 90 : 180;
                float roatationRight = i % 2 == 0 ? 90 : 180;
                float x = i % 4 == 0 ? 11 : 13;
                float scaleLeft = i % 2 == 0 ? 0.5f : 0.3f;
                float scaleRight = i % 3 == 0 ? 0.3f : 0.6f;
                yield return new BuildingA(buildingModel, x, i * 30f + offset, roatationLeft, scaleLeft);
                yield return new BuildingA(buildingModel, -x, i * 30f + offset, roatationRight, scaleRight);
            }
        }

        private IEnumerable<GameObject> GenerateInitialCarAgents()
        {
            for (int i = 0; i < 5; i++)
            {
                yield return _agentFactory.CreateBarrier(0, false, i * 1000 + 50);
                yield return _agentFactory.CreateVan(0, false, i * 200 + 50);
                yield return _agentFactory.CreateVan(0, true, i * 200 + 50);
                yield return _agentFactory.CreateLambo(1, true, i * 200 + 80);
                yield return _agentFactory.CreateLambo(1, false, i * 200 + 80);
                yield return _agentFactory.CreateBus(0, true, i * 300 + 130);
                yield return _agentFactory.CreateBus(0, false, i * 300 + 130);

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


            collision = (gameObjects.OfType<CarAgent>().Any(x => CollisionDetector.IsCollision(x, player)));
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

        public GameState GameState
        {
            get => new GameState() { GameObjects = this.gameObjects.Where(go => go.GetType() == typeof(Car) || go.GetType() == typeof(CarAgent)) };
        }
    }
}
