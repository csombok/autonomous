﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autonomous.Public;
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
        private List<ViewportWrapper> viewports = new List<ViewportWrapper>();
        private List<GameObject> gameObjects = new List<GameObject>();
        private Road road;
        private AgentFactory _agentFactory;
        private CourseObjectFactory courseObjectFactory;
        private PlayerFactory playerFactory;
        private Dashboard dashboard;

        private bool collision;
        private GameStateManager _gameStateManager = new GameStateManager();
        private List<Car> _players;
        private TimeSpan lastUpdate;
        private float _length;
        private float _agentDensity;

        public bool Stopped { get; set; }

        public Game1(float length, float agentDensity)
        {
            _agentDensity = agentDensity;
            _length = length;
            graphics = new GraphicsDeviceManager(this);
            //set the GraphicsDeviceManager's fullscreen property
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            _agentFactory = new AgentFactory(_gameStateManager);
            courseObjectFactory = new CourseObjectFactory();
            playerFactory = new PlayerFactory();
            dashboard = new Dashboard();
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
            Road.LoadContent(Content, graphics);

            _agentFactory.LoadContent(Content);
            courseObjectFactory.LoadContent(Content);
            playerFactory.LoadContent(Content);
            dashboard.LoadContent(Content);
            // TEMP
            InitializeModel();

            viewports = ViewportFactory.CreateViewPorts(_players, 
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight).ToList();

        }

        public void InitializeModel()
        {
            road = new Road();

            _players = playerFactory.LoadPlayers(_gameStateManager).ToList();
            gameObjects = new List<GameObject>(_players) { road };
            gameObjects.AddRange(courseObjectFactory.GenerateCourseArea());
            gameObjects.AddRange(_agentFactory.GenerateInitialCarAgents(_agentDensity));
            gameObjects.ForEach(go => go.Initialize());
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

            try
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();
            }
            catch (Exception e)
            {
            }

            UpdateModel(gameTime);
            viewports.ForEach(vp => vp.Update());
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
            gameObjects.ForEach(go => go.Update(gameTime.ElapsedGameTime));
            UpdateGameCourse(gameTime);
            CheckCollision(gameTime);
        }
        private void CheckCollision(GameTime gameTime)
        {
            collision = false;
            foreach (var player in _players)
            {
                var agentsInCollision = gameObjects
                    .OfType<CarAgent>()
                    .Where(x => CollisionDetector.IsCollision(x, player))
                    .ToList();

                agentsInCollision.ForEach(agent =>
                {
                    agent.HandleCollision(player, gameTime);
                    player.HandleCollision(agent, gameTime);
                });

                collision |= agentsInCollision.Any();
                if (!collision)
                {
                    if (player.X - player.Width / 2 < -6)
                        collision = true;
                    if (player.X + player.Width / 2 > 6)
                        collision = true;
                }
            }
        }

        private void UpdateGameCourse(GameTime gameTime)
        {
            if ((gameTime.TotalGameTime - lastUpdate).TotalMilliseconds < GameConstants.GameCourseUpdateFrequency)
                return;                

            var newObjects = courseObjectFactory
                .GenerateCourseArea(_gameStateManager.GameStateInternal.FirstPlayerPosition)
                .ToList();

            int firstPlayerIndex =
                _gameStateManager.GameStateInternal.GameObjects.IndexOf(
                    _gameStateManager.GameStateInternal.GameObjects.Last(x => x is Car));
            newObjects.AddRange(GenerateAgents(firstPlayerIndex));
            gameObjects.AddRange(newObjects);
            newObjects.ForEach(go => go.Initialize());

            var lastCarPosition = _gameStateManager.GameStateInternal.LastPlayerPosition;

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
            GraphicsDevice.Clear(collision ? Color.Red : Color.CornflowerBlue);

            Viewport original = graphics.GraphicsDevice.Viewport;

            foreach (var viewport in viewports)
            {
                graphics.GraphicsDevice.Viewport = viewport.Viewport;
                Draw(gameTime, viewport);
            }

            graphics.GraphicsDevice.Viewport = original;

            dashboard.DrawPlayerScores(graphics.GraphicsDevice, playerFactory.PlayersInfo);

            base.Draw(gameTime);

        }

        private void Draw(GameTime gameTime, ViewportWrapper viewport)
        {
            gameObjects.ForEach(go => go.Draw(gameTime.ElapsedGameTime, viewport, GraphicsDevice));
            // dashboard.DrawPlayerScores(GraphicsDevice, playerFactory.PlayersInfo);

        }
    }
}
