using GameContracts;
using GameOMICH.Base;
using GameOMICH.GameActors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOMICH.Screens
{
    class GameScreen : ContentProcessorScreen
    {
        private enum Direction { Left, Up, Right, Down };

        private string soundBlock = "chord";
        private string soundStep = "step";
        private string soundWater = "water";
        private string soundVzmah = "vzmah";
        private string soundDrel = "drel";
        private string soundBomb = "bomb";

        private Omich omich;
        private Field mainField;
        private Rocket rocket;
        private Detail detail;

        private string BackGroundContent = "BackGroundGame";

        private bool winAnimation = false;
        private int rocketVelocity = 0;
        private bool firstUpdate = true;
        private long currentTick = 0;
        private TimeSpan elapsed = TimeSpan.Zero;

        public GameScreen(IDevice device) : base(device)
        {
            StartNewGame();

            contentList.AddRange(mainField.GetContent());
            contentList.AddRange(omich.GetContent());
            contentList.AddRange(rocket.GetContent());
            contentList.AddRange(detail.GetContent());

            contentList.Add(new ContentItem() { Type = ContentType.Texture, Name = BackGroundContent });
            contentList.Add(new ContentItem() { Type = ContentType.Sound, Name = soundBlock });
            contentList.Add(new ContentItem() { Type = ContentType.Sound, Name = soundStep });
            contentList.Add(new ContentItem() { Type = ContentType.Sound, Name = soundWater });
            contentList.Add(new ContentItem() { Type = ContentType.Sound, Name = soundVzmah });
            contentList.Add(new ContentItem() { Type = ContentType.Sound, Name = soundDrel });
            contentList.Add(new ContentItem() { Type = ContentType.Sound, Name = soundBomb });
        }

        private void StartNewGame()
        {
            winAnimation = false;
            firstUpdate = true;
            elapsed = TimeSpan.Zero;

            omich = new Omich("omich", 700, 0);
            mainField = new Field("island");
            rocket = new Rocket("rocket", 9, 100, 288, 192);
            detail = new Detail("detail", 8, 600, 0);

            mainField.NextLevel();
        }

        public override ScreenState Update(DateTime gameTime)
        {
            ScreenState ret = ScreenState.Current;

            if (firstUpdate)
            {
                currentTick = gameTime.Ticks;
                firstUpdate = false;
            }

            if (winAnimation)
            {
                if (elapsed == TimeSpan.Zero)
                {
                    elapsed = TimeSpan.FromTicks(gameTime.Ticks - currentTick);
                }
                
                rocket.Y = rocket.Y - Convert.ToSingle(rocketVelocity++ * 0.2);

                mainField.Update(gameTime);

                if (rocket.Y < -192)
                {
                    ret = ScreenState.Win;
                    Updatebalance(ret, elapsed);
                    StartNewGame();
                }

                return ret;
            }
                    
            var inp = device.GetInputState();

            if (inp.IsCancel)
            {
                ret = ScreenState.Menu;
            }
            else
            {
                bool isLive = mainField.GetSquareState(omich.X, omich.Y);
                if (!isLive)
                {
                    if (omich.IsLive())
                    {
                        device.PlaySound(soundWater);
                    }

                    omich.Dead();
                }
                else
                {
                    if (!omich.IsLive())
                    {
                        omich.Enabled = false;
                        ret = ScreenState.Loose;
                    }
                }


                if (omich.IsLive())
                {
                    if (detail.Containts(omich) && detail.Enabled)
                    {
                        detail.Enabled = false;
                        device.PlaySound(soundVzmah);
                    }

                    if (rocket.Containts(omich) && !detail.Enabled)
                    {
                        if (detail.NextFrame())
                        {
                            detail.Enabled = true;
                            mainField.NextLevel();
                        }

                        if (!rocket.NextFrame())
                        {
                            device.PlaySound(soundBomb);
                            omich.Enabled = false;
                            winAnimation = true;
                        }
                        else
                        {
                            device.PlaySound(soundDrel);
                        }
                    }

                    if (inp.Joystick == JoystickState.Down)
                    {
                        OmichMove(Direction.Down);
                    }
                    else if (inp.Joystick == JoystickState.Up)
                    {
                        OmichMove(Direction.Up);
                    }
                    else if (inp.Joystick == JoystickState.Left)
                    {
                        OmichMove(Direction.Left);
                    }
                    else if (inp.Joystick == JoystickState.Right)
                    {
                        OmichMove(Direction.Right);
                    }
                    else if (inp.Joystick == JoystickState.Press)
                    {
                        var Position = inp.Positions.First();

                        if (
                                    (Position.Y <= omich.Y + 96)
                                    && (Position.Y >= omich.Y)
                                    && (Position.X > omich.X + 100)
                            )
                        {
                            OmichMove(Direction.Right);
                        }
                        else if (
                                   (Position.Y <= omich.Y + 96)
                                && (Position.Y >= omich.Y)
                                && (Position.X < omich.X)
                        )
                        {
                            OmichMove(Direction.Left);
                        }
                        else if (
                                    (Position.Y > omich.Y + 96)
                        )
                        {
                            OmichMove(Direction.Down);
                        }
                        else if (
                                    (Position.Y < omich.Y)
                        )
                        {
                            OmichMove(Direction.Up);
                        }
                    }

                    if (omich.IsLive())
                    {
                        if (omich.X == 0)
                            omich.SeeRight();
                        else if (omich.X == 700)
                            omich.SeeLeft();
                    }
                }
                
            }

            mainField.Update(gameTime);
            omich.Update(gameTime);
            rocket.Update(gameTime);
            detail.Update(gameTime);

            if (ret != ScreenState.Current)
            {
                elapsed = TimeSpan.FromTicks(gameTime.Ticks - currentTick);
                Updatebalance(ret, elapsed);
                StartNewGame();
            }

            return ret;
        }

        public override string GetBackgroundColor()
        {
            return "#0000ff";
        }

        public override IEnumerable<ContentDrawable> GetContentDarawable()
        {
            contentDrawList.Clear();

            contentDrawList.AddRange(mainField.GetContentDarawable());

            contentDrawList.Add(new ContentDrawable() { ContentName = BackGroundContent });

            contentDrawList.AddRange(omich.GetContentDarawable());
            contentDrawList.AddRange(rocket.GetContentDarawable());
            contentDrawList.AddRange(detail.GetContentDarawable());

            return contentDrawList;
        }

        private void OmichMove(Direction direction)
        {
            if (direction == Direction.Right)
            {
                double newPos = omich.X + 100;

                if (newPos < 800)
                {
                    if (mainField.IsSquareEnabled(newPos, omich.Y))
                    {
                        device.PlaySound(soundStep);
                        omich.X = newPos;
                    }
                    else
                    {
                        device.PlaySound(soundBlock);
                    }
                }

                omich.SeeRight();
            }
            else if (direction == Direction.Left)
            {
                double newPos = omich.X - 100;

                if (0 <= newPos)
                {
                    if (mainField.IsSquareEnabled(newPos, omich.Y))
                    {
                        device.PlaySound(soundStep);
                        omich.X = newPos;
                    }
                    else
                    {
                        device.PlaySound(soundBlock);
                    }
                }

                omich.SeeLeft();
            }
            else if (direction == Direction.Down)
            {
                double newPos = omich.Y + 96;

                if (newPos < 480)
                {
                    if (mainField.IsSquareEnabled(omich.X, newPos))
                    {
                        device.PlaySound(soundStep);
                        omich.Y = newPos;
                    }
                    else
                    {
                        device.PlaySound(soundBlock);
                    }
                }
            }
            else if (direction == Direction.Up)
            {
                double newPos = omich.Y - 96;

                if (0 <= newPos)
                {
                    if (mainField.IsSquareEnabled(omich.X, newPos))
                    {
                        device.PlaySound(soundStep);
                        omich.Y = newPos;
                    }
                    else
                    {
                        device.PlaySound(soundBlock);
                    }
                }
            }
        }


        private void Updatebalance(ScreenState ret, TimeSpan tsNew)
        {
            string balanceStr = "0";
            string tsStr = "0";
            string promoValue = string.Empty;

            device.LoadSettings("balance", ref balanceStr);
            device.LoadSettings("time", ref tsStr);
            device.LoadSettings("promo", ref promoValue);

            long tsTicks = Convert.ToInt64(tsStr);
            TimeSpan ts = TimeSpan.FromTicks(tsTicks);
            int balance = Convert.ToInt32(balanceStr);
            int balanceNew = balance;


            if (ret == ScreenState.Loose)
                balanceNew--;
            else if (ret == ScreenState.Win)
            {
                balanceNew++;

                if (tsTicks == 0)
                    device.SaveSettings("time", tsNew.Ticks.ToString());
                else if (tsTicks > tsNew.Ticks)
                    device.SaveSettings("time", tsNew.Ticks.ToString());
            }
            
            string promoNew = GetPromo(balance, ts, promoValue, balanceNew, tsNew);

            device.SaveSettings("balance", balanceNew.ToString());
            device.SaveSettings("promo", promoNew);
        }


        private string GetPromo(int balance, TimeSpan ts, string promoValue, int balanceNew, TimeSpan tsNew)
        {
            string ret = promoValue;

            if ((balance < 3) && (balanceNew >= 3))
                ret = "A" + DateTime.Now.ToString("ssHHmm");

            if ((balance < 6) && (balanceNew >= 6))
                ret = "B" + DateTime.Now.ToString("mmHHss");

            if ((balance < 9) && (balanceNew >= 9))
                ret = "M" + DateTime.Now.ToString("HHmmss");


            return ret;
        }

    }
}
