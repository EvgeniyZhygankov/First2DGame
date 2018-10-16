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
    public class Enemy : BaseClass
    {
        int speed;
        float gravity = 0;
        public bool onGround;
        bool onLadder;

        // Работа с frame'ами
        int HorizontalFrame = 0;
        //int VerticalFrame = 0;
        //float TimeForVerticalFrame = 300;
        double TotalTime;
        float TimeForHorizontalFrame = 150;

        // Плазма
        bool fire = true;
        Rectangle findPlayerRectangle;

        #region Работа со звуком

        SoundEffect wallSound;
        SoundEffect plazmaSound;

        #endregion

        // Положение стоя
        bool stay = false;

        int k = -1, ChekSound = -1;

        double Time = 0;

        Hero player;

        int indent = 17;
        public int HelthPoints = 5;

        void Motion(Vector2 speed) { position += speed; }

        public Enemy(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
            wallSound = Game.Content.Load<SoundEffect>("Step2");
            plazmaSound = Game.Content.Load<SoundEffect>("Plazma");
            findPlayerRectangle.Width = 400;
            findPlayerRectangle.Height = 63;
            findPlayerRectangle.Y = (int)position.Y;
        }

        public override void Initialize()
        {
            player = (Hero)Game.Services.GetService(typeof(Hero));

            base.Initialize();
        }

        void PressRight(GameTime gameTime)
        {
            findPlayerRectangle.X = (int)position.X + 32;

            if (onGround)
                if (!CollideWithLadder() || AboveWall())
                {
                    int[] frames = { 128, 160, 192, 224 };
                    TotalTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (TotalTime > TimeForHorizontalFrame)
                    {
                        if (ChekSound < 0)
                        {
                            ChekSound -= 2 * ChekSound;
                            wallSound.Play(0.008f, 0, 0);
                        }
                        else
                            ChekSound -= 2 * ChekSound;

                        switch (HorizontalFrame)
                        {
                            case 0:
                                HorizontalFrame++;
                                break;

                            case 1:
                                HorizontalFrame++;
                                break;

                            case 2:
                                HorizontalFrame++;
                                break;

                            case 3:
                                HorizontalFrame++;
                                break;

                            case 4:
                                HorizontalFrame = 1;
                                break;
                        }
                        rectangle = new Rectangle(frames[HorizontalFrame - 1], 96, 32, 63);
                        TotalTime = 0;
                    }
            }

            Motion(new Vector2(speed, 0));

            //if (CollideWithHero() && fire)
            //    PlazmaFire(new Rectangle(640, 32, 11, 6), new Vector2(position.X + 40, position.Y + 16), new Vector2(5, 0));
        }

        void PressLeft(GameTime gameTime)
        {
            findPlayerRectangle.X = (int)position.X - 400;

            if (!CollideWithLadder() && onGround || AboveWall() && onGround)
            {
                int[] frames = { 352, 320, 288, 256 };
                TotalTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TotalTime > TimeForHorizontalFrame)
                {
                    if (ChekSound < 0)
                    {
                        ChekSound -= 2 * ChekSound;
                        wallSound.Play(0.008f, 0, 0);
                    }
                    else
                        ChekSound -= 2 * ChekSound;

                    switch (HorizontalFrame)
                    {
                        case 0:
                            HorizontalFrame++;
                            break;

                        case 1:
                            HorizontalFrame++;
                            break;

                        case 2:
                            HorizontalFrame++;
                            break;

                        case 3:
                            HorizontalFrame++;
                            break;

                        case 4:
                            HorizontalFrame = 1;
                            break;
                    }
                    rectangle = new Rectangle(frames[HorizontalFrame - 1], 96, 32, 63);
                    TotalTime = 0;
                }
            }

            Motion(new Vector2(-speed, 0));

            //if (CollideWithHero() && fire)
            //    PlazmaFire(new Rectangle(645, 38, 11, 6), new Vector2(position.X - 40, position.Y + 16), new Vector2(-5, 0));
        }

        public override void Update(GameTime gameTime) // UPDATE
        {
            onGround = false;
            onLadder = false;
            speed = 3;
            AboveWall();
            AboveLadder();
            CollideWithLadder();
            GravityAndJump();

            if (CollideWithPlazma() != null)
            {
                HelthPoints--;
                CollideWithPlazma().Dispose();
            }

            if (HelthPoints <= 0)
            {
                Dispose();
                //speed = 0;
                //rectangle = new Rectangle(34, 107, 28, 53);
                //timevalue += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                //if (timevalue > 1000)
            }

            //if (!CollideWithHero())
            //    fire = true;

            if (onGround)
            {
                if (CollideWitInviciblehWall())
                {
                    stay = true;
                    rectangle = new Rectangle(2, 96, 32, 63);
                    Time += gameTime.ElapsedGameTime.Milliseconds;
                    if (Time > 700)
                    {
                        stay = false;
                        k -= 2 * k;
                        Time = 0;
                    }
                }

                if (!stay)
                {
                    if (k < 0)
                        PressLeft(gameTime);

                    if (k > 0)
                        PressRight(gameTime);
                }
            }

            base.Update(gameTime);
        }

        void GravityAndJump()
        {
            if (!onGround && !onLadder)
            { gravity += 0.16f; }

            if (onLadder)
                gravity = 0;

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

        bool CollideWitInviciblehWall()
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

        bool AboveWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + 1 + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    {
                        gravity = 0;
                        onGround = true;
                        return true;
                    }
            }
            return false;
        }

        bool CollideWithLadder()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Ladder))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    {
                        onLadder = true;
                        return true;
                    }
            }
            return false;
        }

        bool AboveLadder()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Ladder))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + 1 + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    {
                        onLadder = true;
                        return true;
                    }
            }
            return false;
        }

        void PlazmaFire(Rectangle RectOfPlazma, Vector2 PosOfPlazma, Vector2 speed)
        {
            //plazmaSound.Play(0.01f, 0, 0);
            Game.Components.Add(new Plazma(Game, ref texture, RectOfPlazma, PosOfPlazma, speed));
            fire = false;
        }

        bool CollideWithHero()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Hero))
                {
                    if (findPlayerRectangle.X + findPlayerRectangle.Width > item.position.X &&
                    findPlayerRectangle.X < item.position.X + item.rectangle.Width &&
                    findPlayerRectangle.Y + findPlayerRectangle.Height > item.position.Y &&
                    findPlayerRectangle.Y < item.position.Y + item.rectangle.Height)
                    { return true; }
                }
            }
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            for (int i = 0; i < HelthPoints; i++)
                spriteBatch.Draw(texture, new Vector2(position.X + i * indent, position.Y - 16), new Rectangle(960, 0, 16, 16), Color.White);

            base.Draw(gameTime);
        }

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
