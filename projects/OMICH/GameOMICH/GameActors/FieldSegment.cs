using GameOMICH.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameContracts;

namespace GameOMICH.GameActors
{
    class FieldSegment : ContentProcessorItem
    {
        public int Num = 0;
        public int SpeedUpdate = 500;
        public bool TheEnd = false;

        private int frameCount;
        private int frameIndex = 0;
        private int incr = 1;

        ContentDrawable drawItem = new ContentDrawable();

        private long lasttime;
        private Random rnd;

        public FieldSegment(string contentBaseName, int frameCount, Random rnd, double X, double Y)
            : base(contentBaseName, X, Y)
        {
            this.frameCount = frameCount - 1;
            this.rnd = rnd;

            for (int i = 0; i <= this.frameCount; i++)
            {
                string contentName = String.Concat(contentBaseName, "_", i);
                contentList.Add(new ContentItem() { Type = ContentType.Texture, Name = contentName });
            }
        }
        
        public override void Update(DateTime gameTime)
        {
            TimeSpan elapsedSpan = new TimeSpan(gameTime.Ticks - lasttime);
            
            if (elapsedSpan.TotalMilliseconds > SpeedUpdate)
            {
                if (frameIndex == 0)
                {
                    int sc = rnd.Next(0, 9);

                    if (0 == sc)
                    {
                        incr = 1;
                    }
                    else
                    {
                        incr = 0;
                    }
                }
                if (frameIndex == frameCount)
                {
                    int sc = rnd.Next(0, 4);

                    if (0 == sc)
                    {
                        incr = -1;
                    }
                    else
                    {
                        incr = 0;
                    }
                }

                frameIndex = frameIndex + incr;

                lasttime = gameTime.Ticks;
            }

            TheEnd = frameIndex == frameCount;
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


    }
}
