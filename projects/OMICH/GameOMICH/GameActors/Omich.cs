using GameOMICH.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameContracts;

namespace GameOMICH.GameActors
{
    class Omich : ContentProcessorItem
    {
        private int frameIndex;

        private const int frameIndexRight = 0;
        private const int frameIndexLeft = 1;
        private const int frameIndexDead = 2;

        ContentDrawable drawItem = new ContentDrawable();

        public Omich(string contentBaseName, double X, double Y) 
            : base(contentBaseName, X, Y)
        {
            string contentBase = String.Concat(contentBaseName, ".", contentBaseName);
            
            for (int i = 0; i < 3; i++)
            {
                string contentName = String.Concat(contentBase, "_", i);
                contentList.Add(new ContentItem() { Type = ContentType.Texture, Name = contentName });
            }

            frameIndex = frameIndexRight;
        }

        public override IEnumerable<ContentDrawable> GetContentDarawable()
        {
            contentDrawList.Clear();

            if (Enabled)
            {
                drawItem.ContentName = contentList[frameIndex].Name;
                drawItem.X = this.X;
                drawItem.Y = this.Y;
                contentDrawList.Add(drawItem);
            }

            return base.GetContentDarawable();
        }

        public void Dead()
        {
            this.frameIndex = frameIndexDead;
        }

        public bool IsLive()
        {
            return this.frameIndex != frameIndexDead;
        }

        public void SeeLeft()
        {
            this.frameIndex = frameIndexLeft;
        }

        public void SeeRight()
        {
            this.frameIndex = frameIndexRight;
        }

    }
}
