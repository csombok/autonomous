using MonoGameTry;
using MonoGameTry.GameObjects;
using System.Collections.Generic;
using System.Linq;

namespace Autonomous.Viewport
{
    class ViewportManager
    {
        private readonly ViewportFactory viewportFactory;

        public ViewportManager(ViewportFactory viewportFactory)
        {
            this.viewportFactory = viewportFactory;
        }

        public List<ViewportWrapper> Viewports { get; private set; }

        public void SetViewports(IList<Car> players)
        {
            Viewports = viewportFactory.CreateViewPorts(players).ToList();
        }
    }
}
