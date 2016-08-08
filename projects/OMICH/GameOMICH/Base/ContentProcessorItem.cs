using System;
using System.Collections.Generic;
using System.Linq;
using GameContracts;

namespace GameOMICH.Base
{
    public class ContentProcessorItem : ContentProcessor
    {
        public Double X { set; get; }
        public Double Y { set; get; }
        public String ContentName { set; get; }
        public bool Enabled { get; set; }

        public ContentProcessorItem() : this(String.Empty, 0, 0) { }

        public ContentProcessorItem(string content, Double X, Double Y)
        {
            this.ContentName = content;
            this.X = X;
            this.Y = Y;

            Enabled = true;
        }

        public virtual void Update(DateTime dt) { }

        public virtual void UpdateInput(IEnumerable<Position> positions) { }

        public virtual bool Containts(ContentProcessorItem target)
        {
            return (this.X == target.X) && (this.Y == target.Y);
        }

    }
}
