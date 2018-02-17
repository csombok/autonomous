using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Autonomous.Commands;
using Autonomous.Public;
using Autonomous.Viewports;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Autonomous.GameObjects;

namespace Autonomous
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private ViewportManager viewportManager;
        private List<GameObject> gameObjects = new List<GameObject>();
        private Road road;
        private FinishLine finishline;
        private AgentFactory _agentFactory;
        private CourseObjectFactory courseObjectFactory;
        private PlayerFactory playerFactory;
        private Dashboard dashboard;

        private GameStateManager _gameStateManager = new GameStateManager();
        private List<Car> _players;
        private TimeSpan lastUpdate;
        private float _length;
        private float _agentDensity;
        private List<IGameCommand> gameCommands = new List<IGameCommand>();
        private bool _slowdown;
        private Texture2D background;
        private FrameCounter _frameCounter = new FrameCounter();

        public bool Stopped { get; set; }

        public Game1(float length, float agentDensity)
        {
            _agentDensity = agentDensity;
            _length = length;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            _agentFactory = new AgentFactory(_gameStateManager);
            courseObjectFactory = new CourseObjectFactory();
            playerFactory = new PlayerFactory();
            dashboard = new Dashboard();
            viewportManager = new ViewportManager(new ViewportFactory(graphics));
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
            background = Content.Load<Texture2D>("background");

            Road.LoadContent(Content, graphics);
            FinishLine.LoadContent(Content, graphics);

            _agentFactory.LoadContent(Content);
            courseObjectFactory.LoadContent(Content);
            playerFactory.LoadContent(Content);
            dashboard.LoadContent(Content);
            // TEMP
            InitializeModel();

            viewportManager.SetViewports(_players);
        }

        public void InitializeModel()
        {
            road = new Road();
            finishline = new FinishLine() {Y = _length};
            _players = playerFactory.LoadPlayers(_gameStateManager).ToList();
            gameObjects = new List<GameObject>(_players) { road, finishline };
            gameObjects.AddRange(courseObjectFactory.GenerateCourseArea());
            gameObjects.AddRange(_agentFactory.GenerateInitialCarAgents(_agentDensity));
            gameObjects.ForEach(go => go.Initialize());

            InitializeCommands();
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
            HandleCommands(gameTime);
            UpdateModel(gameTime);
            viewportManager.Viewports.ForEach(vp => vp.Update());
            
            base.Update(gameTime);
        }

        public void UpdateModel(GameTime gameTime)
        {
            var agentObjects = this.gameObjects
                .Where(go => go.GetType() == typeof(Car) || go.GetType() == typeof(CarAgent)).OrderBy(g => g.Y);
            var internalState = new GameStateInternal() { GameObjects = agentObjects.ToList(), Stopped = Stopped };

            _gameStateManager.GameStateInternal = internalState;
            _gameStateManager.GameState = GameStateMapper.GameStateToPublic(internalState);
            _gameStateManager.GameStateCounter++;

            CheckIfGameFinished(internalState);

            if (_slowdown)
                gameTime.ElapsedGameTime = TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds/5);

            gameObjects.ForEach(go => go.Update(gameTime));
            UpdateGameCourse(gameTime);
            CheckCollision(gameTime);
        }

        private void CheckIfGameFinished(GameStateInternal internalState)
        {
            float firstPlayerFront = internalState.FirstPlayer.BoundingBox.Top;
            if (firstPlayerFront >= finishline.Y - 10)
            {
                _slowdown = true;
                viewportManager.SetViewports(new List<GameObject>() {finishline});
            }

            if (firstPlayerFront >= finishline.Y)
            {
                Stopped = true;
            }

            if (firstPlayerFront >= finishline.Y + 20)
            {
                Exit();
            }
        }

        private void InitializeCommands()
        {
            gameCommands.Add(new ExitCommand(Exit));
            if (playerFactory.HumanPlayerIndex == -1)
            {
                gameCommands.Add(new AutoViewportSelectionCommand(viewportManager, _players, playerFactory.HumanPlayerIndex));
            }
            gameCommands.Add(new ManualViewportSelectionCommand(viewportManager, _players));
        }

        private void HandleCommands(GameTime gameTime)
        {
            gameCommands.ForEach(command => command.Handle(gameTime));
        }

        private void CheckCollision(GameTime gameTime)
        {
            foreach (var player in _players)
            {
                var agentsInCollision = gameObjects
                    .OfType<CarAgent>()
                    .Where(x => CollisionDetector.IsCollision(x, player));

                foreach (var agent in agentsInCollision)
                {
                    agent.HandleCollision(player, gameTime);
                    player.HandleCollision(agent, gameTime);
                }
            }
        }

        private void UpdateGameCourse(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - lastUpdate).TotalMilliseconds < GameConstants.GameCourseUpdateFrequency)
                return;

            var newObjects = courseObjectFactory
                .GenerateCourseArea(_gameStateManager.GameStateInternal.FirstPlayer.Y)
                .ToList();

            int firstPlayerIndex = _gameStateManager.GameStateInternal.FirstPlayerIndex;
            newObjects.AddRange(GenerateAgents(firstPlayerIndex));
            gameObjects.AddRange(newObjects);
            newObjects.ForEach(go => go.Initialize());

            var lastCarPosition = _gameStateManager.GameStateInternal.LastPlayer.Y;

            const float dinstanceToRemove = 50f;

            var objectsToRemove = gameObjects
                .Where(go => !(go.GetType() == typeof(CarAgent) && go.VY >= 0) &&
                             go.GetType() != typeof(Road) &&
                             go.GetType() != typeof(Car))
                .Where(go =>
                    go.BoundingBox.Top <= lastCarPosition &&
                    Math.Abs(go.BoundingBox.Top - lastCarPosition) > dinstanceToRemove
                ).ToList();

            objectsToRemove.ForEach(go => gameObjects.Remove(go));

            lastUpdate = gameTime.ElapsedGameTime;
        }

        private IEnumerable<GameObject> GenerateAgents(int firstPlayerIndex)
        {
            var objects = _gameStateManager.GameStateInternal.GameObjects;
            float? sameDirY = null;
            float? oppositeY = null;


            float firstPlayerPosition = objects[firstPlayerIndex].Y;

            int closeCount = 0;
            for (int i = objects.Count - 1; i > firstPlayerIndex; i--)
            {
                if (objects[i].OppositeDirection && oppositeY == null)
                    oppositeY = objects[i].Y;

                if (!objects[i].OppositeDirection && sameDirY == null)
                {
                    sameDirY = objects[i].Y;
                }

                if (!objects[i].OppositeDirection && objects[i].Y - firstPlayerPosition < 500)
                {
                    closeCount++;
                }
            }

            if (sameDirY - firstPlayerPosition < 500)
                yield return _agentFactory.GenerateRandomAgent(sameDirY.GetValueOrDefault(firstPlayerPosition), false, _agentDensity);

            if (oppositeY - firstPlayerPosition < 500)
                yield return _agentFactory.GenerateRandomAgent(oppositeY.GetValueOrDefault(firstPlayerPosition), true, _agentDensity);

            if (closeCount < 2 + 10 * _agentDensity)
                yield return _agentFactory.GenerateRandomAgent(firstPlayerPosition + 200, false, _agentDensity);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawBackground();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

            Viewport original = graphics.GraphicsDevice.Viewport;

            foreach (var viewport in viewportManager.Viewports)
            {
                graphics.GraphicsDevice.Viewport = viewport.Viewport;
                Draw(gameTime, viewport);
            }

            graphics.GraphicsDevice.Viewport = original;

            dashboard.Draw(graphics.GraphicsDevice, _players, fps);

            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            var backgroundSpriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundSpriteBatch.Begin();

            backgroundSpriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

            backgroundSpriteBatch.End();
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        private void Draw(GameTime gameTime, ViewportWrapper viewport)
        {
            gameObjects.ForEach(go => go.Draw(gameTime.ElapsedGameTime, viewport, GraphicsDevice));
        }
    }
}
