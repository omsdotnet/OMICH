using System;
using System.Collections.Generic;
using GameContracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameDeviceWP
{
    public class DeviceFacadeWP7 : WPXNABase, IDevice
    {
        private Game device;

        public DeviceFacadeWP7(Game xnaGame) : base(xnaGame.Content)
        {
            this.device = xnaGame;
        }

        public void Prepare()
        {
            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(device.GraphicsDevice);
        }


        public void Draws(string colorHex, IEnumerable<ContentDrawable> drawItems)
        {
            Color clearColor = ConvertStringToColor(colorHex);

            device.GraphicsDevice.Clear(clearColor);

            spriteBatch.Begin();

            foreach (var item in drawItems)
            {
                if (item is SpriteSizeble)
                {
                    var element = texturesCache[item.ContentName];
                    var drw = item as SpriteSizeble;
                    var currentBounds = new Rectangle(Convert.ToInt32(drw.X), Convert.ToInt32(drw.Y), Convert.ToInt32(drw.Width), Convert.ToInt32(drw.Height));

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
            device.Exit();
        }

    }
}
