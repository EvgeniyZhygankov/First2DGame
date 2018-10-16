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
    public class Friend : BaseClass
    {
        KeyboardState kbState;

        public bool secondCommand, thirtyCommand;

        Hero player;

        float gravity = 0;

        bool onGround;

        double timeOfShot = 0;
        bool fire = true;

        #region Работа с frame'ами

        int HorizontalFrame = 0;
        float TimeForHorizontalFrame = 200;
        double TotalTime;

        #endregion

        #region Работа со звуом

        SoundEffect wallSound;
        SoundEffect plazmaSound;

        int ChekSound = 1;

        #endregion

        float speed;

        void Motion(Vector2 speed) { position += speed; }

        void PressRight(GameTime gameTime)
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

                if (HorizontalFrame < 4)
                    HorizontalFrame++;
                else
                    HorizontalFrame = 1;

                rectangle = new Rectangle(frames[HorizontalFrame - 1], 96, 32, 63);
                TotalTime = 0;
            }

            Motion(new Vector2(speed, 0));

            while (CollideWithWall() != null)
            { Motion(new Vector2(-1, 0)); }
        }

        void PressLeft(GameTime gameTime)
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

                // как раньше не увидел этого, был switch. Только 21 июля 2016 увидел, код написан в мае или апреле? уже хз, не помню!!.
                // правка внесена утром, перед автошколой и поездкой в кино на фильм Стартрек: Бесконечность.(~11:15 - 11:24).
                if (HorizontalFrame < 4)
                    HorizontalFrame++;
                else
                    HorizontalFrame = 1;

                rectangle = new Rectangle(frames[HorizontalFrame - 1], 96, 32, 63);
                TotalTime = 0;
            }

            Motion(new Vector2(-speed, 0));

            while (CollideWithWall() != null) 
            { Motion(new Vector2(1, 0)); }
        }

        public Friend(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
            player = (Hero)Game.Services.GetService(typeof(Hero));
            wallSound = Game.Content.Load<SoundEffect>("Step2");
        }

        public override void Initialize() { base.Initialize(); }

        void FirstComand()
        {
            secondCommand = false;
            thirtyCommand = false;
            rectangle = new Rectangle(2, 96, 28, 64);
        }

        void SecondComand()
        {
            secondCommand = true;
            thirtyCommand = false;
        }

        void ThirtyComand()
        {
            FirstComand();
            thirtyCommand = true;
        }

        public override void Update(GameTime gameTime)
        {
            speed = 2.5f;
            onGround = false;
            Gravity();
            kbState = Keyboard.GetState();
            player = (Hero)Game.Services.GetService(typeof(Hero));


            if ((position.X + 500 < player.position.X || position.X - 500 > player.position.X) && secondCommand)
            {
                position.Y = player.position.Y;
                position.X = player.position.X - 20;
            }



            if (kbState.IsKeyDown(Keys.D1)) // просто стоять на месте
                FirstComand();



            if (kbState.IsKeyDown(Keys.D2)) // идти за мной
                SecondComand();
            else
                if (secondCommand && !CollideWithHero())
                {
                    if (position.X < player.position.X)
                        PressRight(gameTime);

                    if (position.X > player.position.X)
                        PressLeft(gameTime);

                    if (CollideWithHero())
                        rectangle = new Rectangle(2, 96, 28, 64);
                }



            if (kbState.IsKeyDown(Keys.D3)) // стоять и стрелять
                ThirtyComand();
            else
                if (thirtyCommand)
                    if (CollideWithAlien() != null && fire)
                    {
                        fire = false;
                        if (position.X < CollideWithAlien().position.X)
                            Game.Components.Add(new Plazma(Game, ref texture, new Rectangle(645, 38, 16, 6), new Vector2(position.X + 40, position.Y + 16), new Vector2(5, 0)));

                        if (position.X > CollideWithAlien().position.X)
                            Game.Components.Add(new Plazma(Game, ref texture, new Rectangle(645, 38, 16, 6), new Vector2(position.X - 40, position.Y + 16), new Vector2(-5, 0)));
                    }

            timeOfShot += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeOfShot > 700)
            {
                fire = true;
                timeOfShot = 0;
            }



            base.Update(gameTime);
        }

        void Gravity()
        {
            if (!onGround) { gravity += 0.16f; }

            Motion(new Vector2(0, gravity));

            position.Y = (int)position.Y;

            while (CollideWithWall() != null)
            {
                gravity = 0;
                Motion(new Vector2(0, -1));
            }
        }

        BaseClass CollideWithAlien()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Alien))
                {
                    if (position.X + rectangle.Width + 150 > item.position.X &&
                    position.X - 150 < item.position.X + item.rectangle.Width &&
                    position.Y + rectangle.Height > item.position.Y &&
                    position.Y < item.position.Y + item.rectangle.Height)
                    { return item; }
                }
            }
            return null;
        }

        BaseClass CollideWithWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall) || item.GetType() == typeof(Box))
                {
                    if (position.X + rectangle.Width > item.position.X &&
                    position.X < item.position.X + item.rectangle.Width &&
                    position.Y + rectangle.Height > item.position.Y &&
                    position.Y < item.position.Y + item.rectangle.Height)
                    { return item; }
                }
            }
            return null;
        }

        BaseClass CollideWithBox()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Box))
                {
                    if (position.X + rectangle.Width > item.position.X &&
                    position.X < item.position.X + item.rectangle.Width &&
                    position.Y + rectangle.Height > item.position.Y &&
                    position.Y < item.position.Y + item.rectangle.Height)
                    { return item; }
                }
            }
            return null;
        }

        bool CollideWithHero()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Hero))
                {
                    if (position.X + rectangle.Width + 20 > item.position.X &&
                    position.X - 20 < item.position.X + item.rectangle.Width &&
                    position.Y + rectangle.Height > item.position.Y &&
                    position.Y < item.position.Y + item.rectangle.Height)
                    { return true; }
                }
            }
            return false;
        }
    }
}
