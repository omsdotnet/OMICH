using System;
using System.Collections.Generic;

namespace GameContracts
{
    public class InputState
    {
        public bool IsCancel = false;
        public ICollection<Position> Positions = new List<Position>();
        public JoystickState Joystick = JoystickState.Nothing;
    }
}
