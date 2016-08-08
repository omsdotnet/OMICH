using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameContracts
{
    public abstract class GameBase
    {
        private IDevice device;

        public GameBase(IDevice dev)
        {
            this.device = dev;
        }
    }
}
