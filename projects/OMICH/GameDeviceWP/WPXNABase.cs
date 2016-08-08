using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using GameContracts;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameDeviceWP
{
    public class WPXNABase
    {
        protected SpriteBatch spriteBatch;
        protected ContentManager contentManager;

        protected Dictionary<string, Texture2D> texturesCache = new Dictionary<string, Texture2D>();
        protected Dictionary<string, Song> musicCache = new Dictionary<string, Song>();
        protected Dictionary<string, SoundEffect> soundCache = new Dictionary<string, SoundEffect>();
        protected Dictionary<string, SpriteFont> fontCache = new Dictionary<string, SpriteFont>();

        private SoundEffectInstance currentSound = null;

        private bool gestureProcessing = false;
        private JoystickState lastGesture = JoystickState.Nothing;

        public WPXNABase(ContentManager cntMng)
        {
            this.contentManager = cntMng;
            contentManager.RootDirectory = "Content";

            TouchPanel.EnabledGestures = GestureType.HorizontalDrag |
                                         GestureType.VerticalDrag |
                                         GestureType.DragComplete |
                                         GestureType.Tap;
        }

        public void PrepareContent(IEnumerable<ContentItem> contentItems)
        {
            foreach (var item in contentItems)
            {
                string contentPath = item.Name.Replace(".", @"\");

                if (item.Type == ContentType.Texture)
                {
                    if (!texturesCache.ContainsKey(item.Name))
                    {
                        Texture2D element = contentManager.Load<Texture2D>(contentPath);
                        texturesCache.Add(item.Name, element);
                    }
                }
                else if (item.Type == ContentType.Music)
                {
                    if (!musicCache.ContainsKey(item.Name))
                    {
                        Song element = contentManager.Load<Song>(contentPath);
                        musicCache.Add(item.Name, element);
                    }
                }
                else if (item.Type == ContentType.Sound)
                {
                    if (!soundCache.ContainsKey(item.Name))
                    {
                        SoundEffect element = contentManager.Load<SoundEffect>(contentPath);
                        soundCache.Add(item.Name, element);
                    }
                }
                else if (item.Type == ContentType.Font)
                {
                    if (!fontCache.ContainsKey(item.Name))
                    {
                        SpriteFont element = contentManager.Load<SpriteFont>(contentPath);
                        fontCache.Add(item.Name, element);
                    }
                }

            }
        }

        public void PlayBackgroundMusic(string name)
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                var element = musicCache[name];
                MediaPlayer.Play(element);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void PrepareBackgroundMusic(string name)
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                var element = musicCache[name];
                MediaPlayer.Play(element);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Pause();
            }
        }

        public bool IsPlayBackgroundMusic()
        {
            return MediaPlayer.State == MediaState.Playing;
        }

        public void PauseBackgroundMusic()
        {
            MediaPlayer.Pause();
        }

        public void ResumeBackgroundMusic()
        {
            MediaPlayer.Resume();
        }



        public void PlaySound(string name)
        {
            var element = soundCache[name];

            if (currentSound != null)
                currentSound.Stop();

            currentSound = element.CreateInstance();
            currentSound.Play();
        }

        public void SaveSettings(string key, string value)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings[key] = value;
            settings.Save();
        }

        public bool LoadSettings(string key, ref string value)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            string val;
            bool ret = settings.TryGetValue<string>(key, out val);

            if (ret)
                value = val;

            return ret;
        }

        public void SendEmail(string addr, string subj, string body)
        {
            EmailComposeTask ect = new EmailComposeTask
            {
                To = addr,
                Subject = subj,
                Body = body
            };
            ect.Show();
        }

        public InputState GetInputState()
        {
            var ret = new InputState();

            ret.IsCancel = GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;

            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation item in touchCollection)
            {
                if ((item.State == TouchLocationState.Pressed) || (item.State == TouchLocationState.Moved))
                {
                    var pos = new Position() { X = item.Position.X, Y = item.Position.Y };
                    ret.Positions.Add(pos);
                }
            }

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                if ((gesture.GestureType == GestureType.HorizontalDrag) && !gestureProcessing)
                {
                    if (gesture.Delta.X > 0)
                    {
                        lastGesture = JoystickState.Right;
                        gestureProcessing = true;
                    }
                    else if (gesture.Delta.X < 0)
                    {
                        lastGesture = JoystickState.Left;
                        gestureProcessing = true;
                    }
                }
                else if ((gesture.GestureType == GestureType.VerticalDrag) && !gestureProcessing)
                {
                    if (gesture.Delta.Y > 0)
                    {
                        lastGesture = JoystickState.Down;
                        gestureProcessing = true;
                    }
                    else if (gesture.Delta.Y < 0)
                    {
                        lastGesture = JoystickState.Up;
                        gestureProcessing = true;
                    }
                }
                else if (gesture.GestureType == GestureType.DragComplete)
                {
                    gestureProcessing = false;
                    ret.Joystick = lastGesture;
                }
                else if (gesture.GestureType == GestureType.Tap)
                {
                    ret.Joystick = JoystickState.Press;

                    var pos = new Position() { X = gesture.Position.X, Y = gesture.Position.Y };
                    ret.Positions.Add(pos);
                }
            }

            return ret;
        }

        protected Microsoft.Xna.Framework.Color ConvertStringToColor(String hex)
        {
            //remove the # at the front
            hex = hex.Replace("#", "");

            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;

            //handle ARGB strings (8 characters long)
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }

            //convert RGB characters to bytes
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Microsoft.Xna.Framework.Color.FromNonPremultiplied(r, g, b, a);
        }

    }
}
