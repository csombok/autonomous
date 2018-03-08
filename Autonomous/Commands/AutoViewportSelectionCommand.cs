using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Autonomous.Impl.GameObjects;
using Autonomous.Impl.Viewports;
using Microsoft.Xna.Framework.Input;

namespace Autonomous.Impl.Commands
{
    class AutoViewportSelectionCommand : IGameCommand
    {
        private readonly ViewportManager _viewportManager;
        private readonly List<Car> _players;
        private readonly int _humanPlayerIndex;
        private readonly int _changeInterval;
        private TimeSpan _lastViewportChange;
        private int _activePlayerIndex;
        private int _activeCameraIndex;
        private bool _enabled;
        private bool _showAllPlayersMode = false;

        public AutoViewportSelectionCommand(ViewportManager viewportManager,
            List<Car> players, int humanPlayerIndex, int changeInterval = 6000)
        {
            _viewportManager = viewportManager;
            _players = players;
            _humanPlayerIndex = humanPlayerIndex;
            _changeInterval = changeInterval;
        }

        public void Handle(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F9))
            {
                ToogleEnabled();
            }

            if (!_enabled) return;

            if ((gameTime.TotalGameTime - _lastViewportChange).TotalMilliseconds > _changeInterval)
            {
                _viewportManager.SetViewports(GetActivePlayers(), GetActiveCamera());
                _lastViewportChange = gameTime.TotalGameTime;
            }
        }

        private void ToogleEnabled()
        {
            _enabled = !_enabled;
        }
        
        private IEnumerable<Car> GetActivePlayers()
        {
            if (_players.All(p => p.Stopped)) return _players;

            var activePlayers = _players.Where(p => !p.Stopped).ToList();
            if (_activePlayerIndex % 2 == 0 && _showAllPlayersMode)
            {
                ++_activePlayerIndex;
                _showAllPlayersMode = false;
                return activePlayers;
            }

            _showAllPlayersMode = true;                                
            _activePlayerIndex = _activePlayerIndex >= activePlayers.Count() 
                ? 0 
                :_activePlayerIndex;

            var players = new List < Car > { activePlayers[_activePlayerIndex] };
            ++_activeCameraIndex;
            return players;
        }

        private CameraSetup GetActiveCamera()
        {
            var setup = CameraSetup.CameraDefault;

            if (_activeCameraIndex % 3 == 0) setup = CameraSetup.CameraInside;

            if (_activeCameraIndex % 4 == 0) setup = CameraSetup.CameraRear;

            ++_activeCameraIndex;
            return setup;
        }
    }
}
