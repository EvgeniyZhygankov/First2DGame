using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MakedAtNight
{
    public class Alien : BaseClass
    {
        //Rectangle CollisionRect;

        Hero player;
        float speed = 1f;

        int[] frames2 = { 0, 32, 64, 96, 128 };
        int[] frames = { 128, 96, 64, 32, 0 };

        int HorizontalFrameUp = 0;
        //int HorizontalFrameDown = 0;
        double TotalTime;
        float TimeForHorizontalFrame = 350;

        //bool stay = false;
        //int k = -1;

        //double time = 0;

        //Rectangle findPlayerRectangle;

        //bool canUp = true;

        int indent = 17;
        public int HelthPoints = 3;

        bool onGround;
        float gravity;

        public Alien(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
        }

        public override void Initialize() { base.Initialize(); }

        void Motion(Vector2 speed) { position += speed; }

        //void PressRight(GameTime gameTime)
        //{
        //    //rectangle.X = 192;
        //    //CollisionRect.Height = 22; 

        //    Motion(new Vector2(speed, 0));

        //    //if (CollideWithHero() && fire)
        //        //PlazmaFire(new Rectangle(640, 32, 11, 6), new Vector2(position.X + 40, position.Y + 16), new Vector2(5, 0));
        //}

        //void PressLeft(GameTime gameTime)
        //{
        //    //rectangle.X = 160;
        //    //CollisionRect.Height = 22; 
            
        //    Motion(new Vector2(-speed, 0));

        //    //if (CollideWithHero() && fire)
        //    //PlazmaFire(new Rectangle(645, 38, 11, 6), new Vector2(position.X - 40, position.Y + 16), new Vector2(-5, 0));
        //}

        public override void Update(GameTime gameTime) // UPDATE
        {
            player = (Hero)Game.Services.GetService(typeof(Hero));
            if (position.X <= 50)
            {
                player.HelthPoints = -1;
                Dispose();
            }

            //CollisionRect = new Rectangle((int)position.X, (int)position.Y + (62 - CollisionRect.Height), 32, CollisionRect.Height);
            Motion(new Vector2(-speed, 0));
            AnimationUp(gameTime);

            onGround = false;
            GravityAndJump();

            if (HelthPoints <= 0)
                Dispose();

            if (CollideWithPlazma() != null)
            {
                CollideWithPlazma().Dispose();
                HelthPoints--;
            }
            
            #region старый вариант пришельца, когжа оон ходил и потрулировал, там еще анимация превращения в слизь была.
            
            //if (CollideWithInvisibleWall())
            //{
            //stay = true;

            //if (canUp)
            //    AnimationUp(gameTime);

            //if (HorizontalFrameUp == 5)
            //{
            //    canUp = false;
            //    HorizontalFrameUp = 0;
            //}

            //if (!canUp)
            //    time += gameTime.ElapsedGameTime.TotalMilliseconds;

            //if (time > 1000)
            //{
            //    AnimationDown(gameTime);

            //    if (HorizontalFrameDown == 5)
            //    {
            //        canUp = true;
            //        time = 0;
            //        HorizontalFrameDown = 0;
            //        stay = false;
            //        k -= 2 * k;
            //    }
            //}
            //}

            //if (!stay)
            //{
            //if (k < 0)


            //if (k > 0)
            //    PressRight(gameTime);
            //}

            #endregion

            base.Update(gameTime);
        } 

        void GravityAndJump()
        {
            if (!onGround)
            { gravity += 10f; }

            Motion(new Vector2(0, gravity));

            position.Y = (int)position.Y;

            if (gravity < 0)
                while (CollideWithWall())
                {
                    gravity = 0;
                    Motion(new Vector2(0, 1));
                }
            else
                while (CollideWithWall())
                {
                    gravity = 0;
                    Motion(new Vector2(0, -1));
                }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            for (int i = 0; i < HelthPoints; i++)
                spriteBatch.Draw(texture, new Vector2(position.X + i * indent, position.Y - 16), new Rectangle(640, 44, 16, 16), Color.White);

            base.Draw(gameTime);
        }

        void AnimationUp(GameTime gameTime)
        {
            int[] frames = { 0, 32};
            TotalTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (TotalTime > TimeForHorizontalFrame)
            {
                switch (HorizontalFrameUp)
                {
                    case 0:
                            HorizontalFrameUp++;
                            //CollisionRect.Height = 22;
                        break;

                    case 1:
                            HorizontalFrameUp++;
                            //CollisionRect.Height = 34;
                        break;

                    case 2:
                        HorizontalFrameUp = 1;
                        //CollisionRect.Height = 46;
                        break;

                    //case 3:
                    //        HorizontalFrameUp++;
                    //        CollisionRect.Height = 54;
                    //    break;

                    //case 4:
                    //        HorizontalFrameUp++;
                    //        CollisionRect.Height = 62;
                    //    break;

                    //case 5:
                    //    break;
                }
                rectangle = new Rectangle(frames[HorizontalFrameUp - 1], 256, 32, 64);
                TotalTime = 0;
            }
        }

        //void AnimationDown(GameTime gameTime)
        //{
        //    int[] frames = { 0, 32, 64, 96, 128 };
        //    TotalTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        //    if (TotalTime > TimeForHorizontalFrame)
        //    {
        //        switch (HorizontalFrameDown)
        //        {
        //            case 0:
        //                HorizontalFrameDown++;
        //                CollisionRect.Height = 62;
        //                break;

        //            case 1:
        //                HorizontalFrameDown++;
        //                CollisionRect.Height = 54;
        //                break;

        //            case 2:
        //                HorizontalFrameDown++;
        //                CollisionRect.Height = 46;
        //                break;

        //            case 3:
        //                HorizontalFrameDown++;
        //                CollisionRect.Height = 34;
        //                break;

        //            case 4:
        //                HorizontalFrameDown++;
        //                CollisionRect.Height = 22;
        //                break;

        //            case 5:
        //                break;
        //        }
        //        rectangle = new Rectangle(frames[HorizontalFrameDown - 1], 256, 32, 64);
        //        TotalTime = 0;
        //    }
        //}

        bool CollideWithWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    { return true; }
            }
            return false;
        }

        bool CollideWithInvisibleWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(SpecialForEnemy))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    { return true; }
            }
            return false;
        }

        //void PlazmaFire(Rectangle RectOfPlazma, Vector2 PosOfPlazma, Vector2 speed)
        //{
        //    //plazmaSound.Play(0.01f, 0, 0);
        //    Game.Components.Add(new Plazma(Game, ref texture, RectOfPlazma, PosOfPlazma, speed));
        //    fire = false;
        //}

        BaseClass CollideWithPlazma()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Plazma))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    { return item; }
            }
            return null;
        }
    }
}