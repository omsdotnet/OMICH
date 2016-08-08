using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameContracts
{
    public interface IGame
    {
        void Prepare();
        void Update(DateTime currentTime);
        void Draw();
    }
}
