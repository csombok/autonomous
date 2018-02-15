using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autonomous.GameObjects;

namespace Autonomous.Strategies
{
    public interface IControlStrategy
    {
        ControlState Calculate();

        // TODO find a better design
        GameObject GameObject { get; set; }
    }
}
