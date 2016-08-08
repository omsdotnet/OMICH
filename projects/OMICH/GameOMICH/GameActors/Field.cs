using GameOMICH.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameContracts;

namespace GameOMICH.GameActors
{
    class Field : ContentProcessorItem
    {
        private const double Width = 100;
        private const double Height = 96;
        
        List<int> allItems = new List<int>() { 1,  2,  3,  5,  6,  7,  8, 
                                               10, 11, 12, 13, 16, 17, 18, 19,
                                               21, 22, 23, 24, 26, 27, 28, 29,
                                               31, 32, 33, 34, 36, 37, 38, 39 };

        List<int> blockItems = new List<int>() { 0, 4, 9, 14, 15, 20, 25, 30, 35 };
        List<int> disabledItems = new List<int>() { 0, 14, 15, 20, 25 };
        List<int> processedItems = new List<int>();

        FieldSegment[,] arr;

        Random rnd = new Random();

        public Field(string contentBaseName) 
            : base()
        {
            int scX = 8;
            int scY = 5;
            int sc = 0;

            arr = new FieldSegment[scX, scY];

            string SegmentContentName = String.Concat(contentBaseName, ".", contentBaseName);

            for (int i = 0; i < scX; i++)
            {
                for (int j = 0; j < scY; j++)
                {
                    arr[i, j] = new FieldSegment(SegmentContentName, 9, rnd, i * Width, j * Height);
                    arr[i, j].Num = sc;

                    sc++;
                }
            }

            contentList.AddRange(arr[0, 0].GetContent());
        }

        public override void Update(DateTime gameTime)
        {
            foreach (FieldSegment item in arr)
            {
                if (!blockItems.Contains(item.Num) && processedItems.Contains(item.Num))
                {
                    item.Update(gameTime);
                }
            }
        }

        public override IEnumerable<ContentDrawable> GetContentDarawable()
        {
            contentDrawList.Clear();

            if (Enabled)
            {
                foreach (FieldSegment item in arr)
                {
                    contentDrawList.AddRange(item.GetContentDarawable());
                }
            }

            return base.GetContentDarawable();
        }

        public void NextLevel()
        {
            int speedDecrease = 3;

            for (int i = 0; i < 5; i++)
            {
                int rndPos = rnd.Next(0, allItems.Count);

                if (allItems.Count > 0)
                {
                    int item = allItems[rndPos];
                    allItems.Remove(item);
                    processedItems.Add(item);
                }
                else
                {
                    speedDecrease = 30;
                }
            }

            foreach (FieldSegment item in arr)
            {
                item.SpeedUpdate -= speedDecrease;
            }
        }

        public bool GetSquareState(double x, double y)
        {
            int i = (int)(x / Width);
            int j = (int)(y / Height);

            bool ret = true;

            if (!blockItems.Contains(arr[i, j].Num))
            {

                if (arr[i, j].TheEnd)
                {
                    ret = false;
                }
            }

            return ret;
        }

        public bool IsSquareEnabled(double x, double y)
        {
            int i = (int)(x / Width);
            int j = (int)(y / Height);


            return !disabledItems.Contains(arr[i, j].Num);
        }

    }
}
