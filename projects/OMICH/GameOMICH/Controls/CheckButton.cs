using System;
using System.Collections.Generic;
using System.Linq;
using GameContracts;
using GameOMICH.Base;

namespace GameOMICH.Controls
{
    public class CheckButton : ContentProcessorItem
    {
        string contentUncheckName;
        double Width;
        double Height;
        ButtonState Status;
        SpriteSizeble drawItem = new SpriteSizeble();

        public event EventHandler Clicked;
        public event EventHandler Down;
        public bool IsChecked = false;


        public CheckButton(string contentCheckName, string contentUncheckName, double X, double Y, double Width, double Height)
            : base(contentCheckName, X, Y)
        {
            this.contentUncheckName = contentUncheckName;
            this.Width = Width;
            this.Height = Height;

            Status = ButtonState.Up;

            contentList.Add(new ContentItem() { Type = ContentType.Texture, Name = contentCheckName });
            contentList.Add(new ContentItem() { Type = ContentType.Texture, Name = contentUncheckName });
        }

        public override void UpdateInput(IEnumerable<Position> positions)
        {
            if (Enabled)
            {
                if (Status == ButtonState.Up)
                {
                    foreach (var item in positions)
                    {
                        if (ContainsPos(item))
                        {
                            Status = ButtonState.Down;

                            if (Down != null)
                            {
                                Down(this, EventArgs.Empty);
                            }
                        }
                    }
                }
                else
                {
                    if (positions.Count() == 0)
                    {
                        IsChecked = !IsChecked;

                        Status = ButtonState.Up;

                        if (Clicked != null)
                        {
                            Clicked(this, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        int scOutTouches = 0;
                        int scPressedTouches = 0;
                        foreach (var item in positions)
                        {
                            scPressedTouches++;
                                
                            if (!ContainsPos(item))
                            {
                                scOutTouches++;
                            }
                        }

                        if ((scPressedTouches == scOutTouches) && (scPressedTouches > 0))
                        {
                            Status = ButtonState.Up;
                        }
                    }
                }
            }
        }

        public override IEnumerable<ContentDrawable> GetContentDarawable()
        {
            contentDrawList.Clear();

            if (Enabled)
            {
                drawItem.ContentName = IsChecked ? this.ContentName : this.contentUncheckName;

                if (Status == ButtonState.Up)
                {
                    drawItem.X = this.X;
                    drawItem.Y = this.Y;
                    drawItem.Width = this.Width;
                    drawItem.Height = this.Height;
                }
                else
                {
                    drawItem.X = this.X + 6;
                    drawItem.Y = this.Y + 6;
                    drawItem.Width = this.Width - 12;
                    drawItem.Height = this.Height - 12;
                }

                contentDrawList.Add(drawItem);
            }

            return base.GetContentDarawable();
        }

        protected bool ContainsPos(Position pos)
        {
            bool ret = (pos.X >= this.X) && (pos.X <= (this.X + this.Width)) &&
                       (pos.Y >= this.Y) && (pos.Y <= (this.Y + this.Height));

            return ret;
        }
    }

}
