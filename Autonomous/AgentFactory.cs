using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameTry.GameObjects;
using MonoGameTry.Strategies;

namespace MonoGameTry
{
    class AgentFactory
    {
        private Model _vanModel;
        private Model _lamboModel;
        private Model _busModel;
        private Model _barrierModel;
        private IGameStateProvider _gameStateProvider;
        private Random random = new Random();

        public AgentFactory(IGameStateProvider stateProvider)
        {
            _gameStateProvider = stateProvider;
        }
        public void LoadContent(ContentManager content)
        {
            _vanModel = content.Load<Model>("kendo");
            _lamboModel = content.Load<Model>("Lambo\\Lamborghini_Aventador");
            _busModel = content.Load<Model>("bus");
            _barrierModel = content.Load<Model>("barrier");
        }

        const float laneWidth = GameConstants.LaneWidth;
        public CarAgent CreateBarrier(int lane, bool opposite, float y)
        {
            var barrier = new CarAgent(_barrierModel, 90, laneWidth * 0.8f, opposite, _gameStateProvider, null)
            {
                VY = 0,
                MaxVY = 0,
                X = GetLaneCenter(lane, opposite),
                Y = y
            };
            return barrier;
        }

        public CarAgent CreateVan(int lane, bool opposite, float y)
        {
            const float vanWidth = 2f;

            float v = ((float)random.NextDouble() * 20 + 70) / 3.6f;
            return CreateVehicleAgent(_vanModel, lane, opposite, y, vanWidth, v, 90);
        }

        public CarAgent CreateLambo(int lane, bool opposite, float y)
        {
            const float width = 2f;

            float v = ((float)random.NextDouble() * 20 + 110) / 3.6f;
            return CreateVehicleAgent(_lamboModel, lane, opposite, y, width, v, 180);
        }

        public CarAgent CreateBus(int lane, bool opposite, float y)
        {
            const float width = 2.6f;

            float v = ((float)random.NextDouble() * 20 + 50) / 3.6f;
            return CreateVehicleAgent(_busModel, lane, opposite, y, width, v, 180);
        }

        private CarAgent CreateVehicleAgent(Model model, int lane, bool opposite, float y, float vanWidth, float v, float rotation)
        {
            var van = new CarAgent(model, rotation, vanWidth, opposite, _gameStateProvider,
                new OvertakingStrategy(v, lane, _gameStateProvider))
            {
                VY = v,
                MaxVY = 100f / 3.6f,
                X = GetLaneCenter(lane, opposite),
                Y = y
            };
            return van;
        }

        private float GetLaneCenter(int lane, bool opposite)
        {
            float x = 0;
            if (lane == 0)
                x = GameConstants.LaneWidth * 1.45f;
            else
                x = GameConstants.LaneWidth / 2;

            if (opposite)
                x = -x;

            return x;
        }
    }
}
