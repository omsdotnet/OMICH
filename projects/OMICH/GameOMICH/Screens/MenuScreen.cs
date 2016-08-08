using GameContracts;
using GameOMICH.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameOMICH.Controls;

namespace GameOMICH.Screens
{
    class MenuScreen : ContentProcessorScreen
    {
        private ScreenState currentScreenState = ScreenState.Current;

        private Button prologBtn;
        private Button playBtn;
        private CheckButton musicBtn;
        private Button authorsBtn;
        private Button exitBtn;

        private Label scoreLblCaption;
        private Label scoreLblValue;
        private Label timeLblCaption;
        private Label timeLblValue;
        private Label promoLblCaption;
        private Label promoLblValue;


        public MenuScreen(IDevice device) : base(device)
        {
            prologBtn = new Button("menu.prolog", 6, 6, 148, 151);
            prologBtn.Clicked += prologBtnClicked;
            playBtn = new Button("menu.play", 160, 0, 480, 480);
            playBtn.Clicked += playBtnClicked;
            musicBtn = new CheckButton("menu.music_on", "menu.music_off", 6, 164, 148, 151);
            musicBtn.Clicked += musicBtnClicked;
            authorsBtn = new Button("menu.authors", 6, 323, 148, 151);
            authorsBtn.Clicked += authorsBtnClicked;
            exitBtn = new Button("menu.exit", 646, 323, 148, 151);
            exitBtn.Clicked += exitBtnClicked;

            if (device.IsPlayBackgroundMusic())
            {
                musicBtn.IsChecked = false;
            }
            else
            {
                string isPlayValue = "Y";
                device.LoadSettings("music", ref isPlayValue);
                musicBtn.IsChecked = isPlayValue != "Y";
            }

            scoreLblCaption = new Label("Balance:", 646, 6, "Arial");
            scoreLblValue = new Label(String.Empty, 646, 36, "Arial");

            timeLblCaption = new Label("Best time:", 646, 106, "Arial");
            timeLblValue = new Label(String.Empty, 646, 136, "Arial");

            promoLblCaption = new Label("Promo:", 646, 206, "Arial");
            promoLblValue = new Label(String.Empty, 646, 236, "Arial");

            contentList.AddRange(prologBtn.GetContent());
            contentList.AddRange(playBtn.GetContent());
            contentList.AddRange(musicBtn.GetContent());
            contentList.AddRange(authorsBtn.GetContent());
            contentList.AddRange(exitBtn.GetContent());
            contentList.AddRange(scoreLblCaption.GetContent());
            contentList.AddRange(scoreLblValue.GetContent());
            contentList.AddRange(timeLblCaption.GetContent());
            contentList.AddRange(timeLblValue.GetContent());
            contentList.AddRange(promoLblCaption.GetContent());
            contentList.AddRange(promoLblValue.GetContent());
        }

        public override ScreenState Update(DateTime gameTime)
        {
            ScreenState ret = ScreenState.Current;

            var inp = device.GetInputState();

            if (inp.IsCancel)
            {
                ret = ScreenState.Exit;
            }
            else
            {
                prologBtn.UpdateInput(inp.Positions);
                playBtn.UpdateInput(inp.Positions);
                musicBtn.UpdateInput(inp.Positions);
                authorsBtn.UpdateInput(inp.Positions);
                exitBtn.UpdateInput(inp.Positions);

                string balance = "0";
                device.LoadSettings("balance", ref balance);
                string balanceValue;
                if (balance.Length >= 7)
                {
                    if (Convert.ToInt32(balance) > 0)
                        balanceValue = "*******";
                    else
                        balanceValue = "-*******";
                }
                else balanceValue = balance;
                scoreLblValue.Text = balanceValue;

                string timeValue = "-";
                device.LoadSettings("time", ref timeValue);
                if (timeValue != "-")
                {
                    long ticks = Convert.ToInt64(timeValue);
                    TimeSpan ts = TimeSpan.FromTicks(ticks);
                    timeValue = ts.ToString("mm\\:ss");
                }
                timeLblValue.Text = timeValue;

                string promoValue = "-";
                device.LoadSettings("promo", ref promoValue);
                promoLblValue.Text = promoValue;

                ret = currentScreenState;
                currentScreenState = ScreenState.Current;
            }

            return ret;
        }

        public override IEnumerable<ContentDrawable> GetContentDarawable()
        {
            contentDrawList.Clear();

            contentDrawList.AddRange(prologBtn.GetContentDarawable());
            contentDrawList.AddRange(playBtn.GetContentDarawable());
            contentDrawList.AddRange(musicBtn.GetContentDarawable());
            contentDrawList.AddRange(authorsBtn.GetContentDarawable());
            contentDrawList.AddRange(exitBtn.GetContentDarawable());
            contentDrawList.AddRange(scoreLblCaption.GetContentDarawable());
            contentDrawList.AddRange(scoreLblValue.GetContentDarawable());
            contentDrawList.AddRange(timeLblCaption.GetContentDarawable());
            contentDrawList.AddRange(timeLblValue.GetContentDarawable());
            contentDrawList.AddRange(promoLblCaption.GetContentDarawable());
            contentDrawList.AddRange(promoLblValue.GetContentDarawable());

            return contentDrawList;
        }


        private void exitBtnClicked(object sender, EventArgs e)
        {
            currentScreenState = ScreenState.Exit;
        }

        private void playBtnClicked(object sender, EventArgs e)
        {
            currentScreenState = ScreenState.Game;
        }

        private void prologBtnClicked(object sender, EventArgs e)
        {
            currentScreenState = ScreenState.Intro;
        }

        private void musicBtnClicked(object sender, EventArgs e)
        {
            if (musicBtn.IsChecked)
            {
                device.PauseBackgroundMusic();
                device.SaveSettings("music", "N");
            }
            else
            {
                device.ResumeBackgroundMusic();
                device.SaveSettings("music", "Y");
            }
        }

        private void authorsBtnClicked(object sender, EventArgs e)
        {
            device.SendEmail("bcwd_support@hotmail.com", "OMICH", String.Empty);
        }

    }
}
