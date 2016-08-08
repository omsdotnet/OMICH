using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameContracts;
using GameOMICH.Base;
using GameOMICH.Screens;

namespace GameOMICH
{
    public class GameFacade : GameBase, IGame
    {
        private IDevice device;
        private Dictionary<ScreenState, ContentProcessorScreen> screenSwitcher;
        private ContentProcessorScreen currentScreen;
        private string isMusicPlay;

        public GameFacade(IDevice dev) : base(dev) 
        {
            device = dev;

            isMusicPlay = "Y";
            bool isFirstStart = !device.LoadSettings("music", ref isMusicPlay);
            if (isFirstStart)
                device.SaveSettings("music", isMusicPlay);

            screenSwitcher = new Dictionary<ScreenState, ContentProcessorScreen>
            {
                { ScreenState.Menu, new MenuScreen(device) },
                { ScreenState.Game, new GameScreen(device) },
                { ScreenState.Intro, new ComicsScreen(device, "intro", 17) },
                { ScreenState.Loose, new  ComicsScreen(device,"loose", 10) },
                { ScreenState.Win, new ComicsScreen(device, "win", 10) },
                { ScreenState.Exit, null }
            };

            currentScreen = isFirstStart ? screenSwitcher[ScreenState.Intro] 
                                         : screenSwitcher[ScreenState.Menu];
        }

        public void Prepare()
        {
            device.Prepare();

            var list = new List<ContentItem>();
            list.Add(new ContentItem() { Type = ContentType.Music, Name = "music" });

            foreach (var item in screenSwitcher)
            {
                if (item.Value != null)
                {
                    list.AddRange(item.Value.GetContent());
                }
            }

            device.PrepareContent(list);

            if (isMusicPlay == "Y")
                device.PlayBackgroundMusic("music");
            else
                device.PrepareBackgroundMusic("music");
        }

        public void Update(DateTime currentTime)
        {
            ScreenState scrSate = currentScreen.Update(currentTime);

            if (scrSate != ScreenState.Current)
                currentScreen = screenSwitcher[scrSate];

            if (currentScreen == null)
                device.Exit();
        }

        public void Draw()
        {
            if (currentScreen != null)
                device.Draws(currentScreen.GetBackgroundColor(), currentScreen.GetContentDarawable());
        }
    }
}
