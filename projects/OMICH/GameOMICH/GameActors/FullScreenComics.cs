using System;
using GameOMICH.Base;
using GameContracts;
using System.Collections.Generic;

namespace GameOMICH.GameActors
{
    public class FullScreenComics : ContentProcessorItem
    {
        private int frameCount;
        private int frameIndex = 0;

        private const int waitSeconds = 5;

        private bool firstLock = false;
        private long lasttime;

        ContentDrawable drawItem = new ContentDrawable();

        public bool TheEnd = false;


        public FullScreenComics(string contentBaseName, int frameCount)
            : base()
        {
            this.frameCount = frameCount - 1;

            for (int i = 0; i <= this.frameCount; i++)
            {
                string contentName = String.Concat(contentBaseName, "_", i);
                contentList.Add(new ContentItem() { Type = ContentType.Texture, Name = contentName });
            }
        }

        public override void Update(DateTime gameTime)
        {
            if (!firstLock)
            {
                firstLock = true;
                lasttime = gameTime.Ticks;
            }

            TimeSpan elapsedSpan = new TimeSpan(gameTime.Ticks - lasttime);

            if (elapsedSpan.TotalSeconds > waitSeconds)
            {
                if (frameIndex < frameCount)
                    frameIndex++;
                else
                    TheEnd = true;

                lasttime = gameTime.Ticks;
            }
        }

        public bool NextFrame()
        {
            bool ret = true;

            if (frameIndex < frameCount)
                frameIndex++;
            else
                ret = false;

            return ret;
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

        public void UpdateTimer(DateTime gameTime)
        {
            lasttime = gameTime.Ticks;
        }

        public void Reset()
        {
            lasttime = 0;
            firstLock = false;
            TheEnd = false;
            frameIndex = 0;
        }

    }
}
