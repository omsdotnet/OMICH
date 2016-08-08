using GameContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOMICH.Base
{
    public abstract class ContentProcessor
    {
        protected List<ContentItem> contentList = new List<ContentItem>();
        protected List<ContentDrawable> contentDrawList = new List<ContentDrawable>();

        public virtual IEnumerable<ContentItem> GetContent()
        {
            return contentList;
        }

        public virtual IEnumerable<ContentDrawable> GetContentDarawable()
        {
            return contentDrawList;
        }
    }
}
