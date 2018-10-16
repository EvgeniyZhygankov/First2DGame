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
    public class Box : BaseClass
    {
        float gravity = 0, rotation;

        bool onGround;

        SoundEffect Push;

        int TotalTime = 0;

        int speed = 3;

        void Motion(Vector2 speed) { position += speed; }

        public Box(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
            Push = Game.Content.Load<SoundEffect>("PushBox");
        }

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime)
        {
            onGround = false;
            AboveWall();
            Gravity();

            if (RightOrLeftWall() != null && onGround)
            {
                if (position.X < RightOrLeftWall().position.X)
                    Motion(new Vector2(-1, 0));
                else
                    if (position.X > RightOrLeftWall().position.X)
                        Motion(new Vector2(1, 0));
            }

            while (AroundBox() != null)
            {
                if (position.X < AroundBox().position.X)
                {
                    Motion(new Vector2(-speed, 0));
                    TotalTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (TotalTime >= 500)
                    {
                        TotalTime = 0;
                        Push.Play(0.005f, 0, 0);
                    }
                }
                else
                    if (position.X > AroundBox().position.X)
                    {
                        Motion(new Vector2(speed, 0));
                        TotalTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                        if (TotalTime >= 500)
                        {
                            TotalTime = 0;
                            Push.Play(0.005f, 0, 0);
                        }
                    }
            }

            base.Update(gameTime); 
        }

        void Gravity()
        {
            if (!onGround) { gravity += 0.16f; }

            Motion(new Vector2(0, gravity));

            position.Y = (int)position.Y;

            while (CollideWithWall())
            {
                gravity = 0;
                Motion(new Vector2(0, -1));
            }
        }

        bool AboveWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall) || (item.GetType() == typeof(Box) && item != this))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + 1 + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    {
                        rotation = 0;
                        gravity = 0;
                        onGround = true;
                        return true;
                    }
            }
            return false;
        }

        bool CollideWithWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall) || (item.GetType() == typeof(Box) && item != this))
                {
                    if (position.X + rectangle.Width > item.position.X &&
                    position.X < item.position.X + item.rectangle.Width &&
                    position.Y + rectangle.Height > item.position.Y &&
                    position.Y < item.position.Y + item.rectangle.Height)
                    { return true; }
                }
            }
            return false;
        }

        BaseClass RightOrLeftWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall) || (item.GetType() == typeof(Box) && item != this))
                {
                    if (position.X + 40 + rectangle.Width > item.position.X &&
                    position.X - 40 < item.position.X + item.rectangle.Width &&
                    position.Y + rectangle.Height > item.position.Y &&
                    position.Y < item.position.Y + item.rectangle.Height)
                    { return item; }
                }
            }
            return null;
        }

        BaseClass AroundBox()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Hero))
                {
                    if (position.X + 1 + rectangle.Width > item.position.X &&
                        position.X - 1 < item.position.X + item.rectangle.Width &&
                        position.Y + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    { return item; }
                }
            }
            return null;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            spriteBatch.Draw(texture, position, rectangle, Color.White, rotation, new Vector2(0, 0), 1, SpriteEffects.None, 0f);
        }
    }
}
