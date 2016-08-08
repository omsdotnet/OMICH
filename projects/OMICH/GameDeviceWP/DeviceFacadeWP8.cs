using GameContracts;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameDeviceWP
{
    public class DeviceFacadeWP8 : WPXNABase, IDevice
    {
        private PhoneApplicationPage device;

        public DeviceFacadeWP8(PhoneApplicationPage dev, ContentManager contentManager) : base(contentManager)
        {
            this.device = dev;
        }

        public void Prepare()
        {
            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
        }


        public void Draws(string colorHex, IEnumerable<ContentDrawable> drawItems)
        {
            Microsoft.Xna.Framework.Color clearColor = ConvertStringToColor(colorHex);

            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            spriteBatch.Begin();

            foreach (var item in drawItems)
            {
                if (item is SpriteSizeble)
                {
                    var element = texturesCache[item.ContentName];
                    var drw = item as SpriteSizeble;
                    var currentBounds = new Microsoft.Xna.Framework.Rectangle(Convert.ToInt32(drw.X), Convert.ToInt32(drw.Y), Convert.ToInt32(drw.Width), Convert.ToInt32(drw.Height));

                    spriteBatch.Draw(element, currentBounds, Microsoft.Xna.Framework.Color.White);
                }
                else if (item is TextDrawable)
                {
                    var element = fontCache[item.ContentName];
                    var drw = item as TextDrawable;
                    var pos = new Vector2(Convert.ToSingle(item.X), Convert.ToSingle(item.Y));

                    spriteBatch.DrawString(element, drw.Text, pos, Microsoft.Xna.Framework.Color.White);
                }
                else
                {
                    var element = texturesCache[item.ContentName];
                    var pos = new Vector2(Convert.ToSingle(item.X), Convert.ToSingle(item.Y));

                    spriteBatch.Draw(element, pos, Microsoft.Xna.Framework.Color.White);
                }
            }

            spriteBatch.End();
        }

        public void Exit()
        {
            new Microsoft.Xna.Framework.Game().Exit();
        }

    }
}
