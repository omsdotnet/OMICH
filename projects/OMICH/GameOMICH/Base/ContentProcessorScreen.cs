using GameContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOMICH.Base
{
    class ContentProcessorScreen : ContentProcessor
    {
        protected IDevice device;

        public ContentProcessorScreen(IDevice device)
        {
            this.device = device;
        }

        public virtual ScreenState Update(DateTime gameTime)
        {
            ScreenState ret = ScreenState.Current;

            var inp = device.GetInputState();

            if (inp.IsCancel)
                ret = ScreenState.Exit;


            return ret;
        }

        public virtual string GetBackgroundColor()
        {
            return "#000000";
        }
    }
}
