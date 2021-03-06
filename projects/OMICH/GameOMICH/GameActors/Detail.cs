﻿using GameOMICH.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameContracts;

namespace GameOMICH.GameActors
{
    class Detail : ContentProcessorItem
    {
        private int frameCount;
        private int frameIndex = 0;

        ContentDrawable drawItem = new ContentDrawable();

        public Detail(string contentBaseName, int frameCount, double X, double Y)
            : base(contentBaseName, X, Y)
        {
            this.frameCount = frameCount - 1;

            string contentBase = String.Concat(contentBaseName, ".", contentBaseName);

            for (int i = 0; i <= this.frameCount; i++)
            {
                string contentName = String.Concat(contentBase, "_", i);
                contentList.Add(new ContentItem() { Type = ContentType.Texture, Name = contentName });
            }
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

        public bool NextFrame()
        {
            bool ret = true;

            if (frameIndex < frameCount)
                frameIndex++;
            else
                ret = false;

            return ret;
        }


    }
}
