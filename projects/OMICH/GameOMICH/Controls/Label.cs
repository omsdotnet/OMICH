using System;
using System.Collections.Generic;
using System.Linq;
using GameContracts;
using GameOMICH.Base;

namespace GameOMICH.Controls
{
    public class Label : ContentProcessorItem
    {
        public string Text;

        string fontContentName;
        TextDrawable drawItem = new TextDrawable();

        public Label(string text, double X, double Y, string fontContentName) 
            : base(fontContentName, X, Y)
        {
            this.Text = text;
            this.fontContentName = fontContentName;

            contentList.Add(new ContentItem() { Type = ContentType.Font, Name = fontContentName });
        }

        public override IEnumerable<ContentDrawable> GetContentDarawable()
        {
            contentDrawList.Clear();

            if (Enabled)
            {
                drawItem.X = this.X;
                drawItem.Y = this.Y;
                drawItem.ContentName = this.fontContentName;
                drawItem.Text = this.Text;

                contentDrawList.Add(drawItem);
            }

            return base.GetContentDarawable();
        }
    }
}
