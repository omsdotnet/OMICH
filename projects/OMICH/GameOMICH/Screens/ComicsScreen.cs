using GameContracts;
using GameOMICH.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameOMICH.GameActors;

namespace GameOMICH.Screens
{
    class ComicsScreen : ContentProcessorScreen
    {
        private bool isTouchPressed = false;
        private FullScreenComics comics;

        public ComicsScreen(IDevice device, string contentGroupName, int framesCount)
            : base(device)
        {
            comics = new FullScreenComics(String.Concat(contentGroupName, ".", contentGroupName), framesCount);

            contentList.AddRange(comics.GetContent());
        }

        public override ScreenState Update(DateTime gameTime)
        {
            ScreenState ret = ScreenState.Current;

            var inp = device.GetInputState();

            if (inp.IsCancel)
            {
                ret = ScreenState.Menu;
            }
            else
            {
                bool isTouchPressedCurrent = inp.Positions.Any();

                if (!isTouchPressed)
                {
                    if (isTouchPressedCurrent)
                        comics.UpdateTimer(gameTime);
                    else
                        if (comics.TheEnd)
                            ret = ScreenState.Menu;

                }
                else
                {
                    if (!isTouchPressedCurrent)
                    {
                        if (!comics.NextFrame())
                            ret = ScreenState.Menu;
                    }
                    else
                        comics.UpdateTimer(gameTime);
                }

                isTouchPressed = isTouchPressedCurrent;
               
                comics.Update(gameTime);
            }

            if (ret != ScreenState.Current)
            {
                comics.Reset();
            }
            
            return ret;
        }

        public override IEnumerable<ContentDrawable> GetContentDarawable()
        {
            return comics.GetContentDarawable();
        }
    }
}
