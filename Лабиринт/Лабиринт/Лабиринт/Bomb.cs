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
    public class Bomb : BaseClass
    {
        //double totalTime = 0;
        //int FrameOfBomb = 0;
        float gravity = -4f;
        bool onGround;
        SoundEffect BombGravity;
        int time = 0;
        
        public Bomb(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            rectangle = newRectangle;
            position = newPosition;
            texture = newTexture;
            BombGravity = Game.Content.Load<SoundEffect>("Grenade Hit");
        }

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime)
        {
            onGround = false;
            Gravity();
            AboveWall();
            time += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (time > 3000)
            {
                Game.Components.Add(new Explosive(Game, ref texture, new Rectangle(256, 0, 32, 32), new Vector2(position.X, position.Y)));
                this.Dispose();
            }

            //int[] FramesOfBomb = {192, 224, 256, 288, 320, 352, 384, 416, 448};
            //double time = 0.1;
            //totalTime += gameTime.ElapsedGameTime.TotalSeconds;
            //if (totalTime > time)
            //{
            //    time += 0.1;

            //    switch (FrameOfBomb)
            //    {
            //        case 0:
            //            FrameOfBomb++;
            //            break;

            //        case 1:
            //            FrameOfBomb++;
            //            break;

            //        case 2:
            //            FrameOfBomb++;
            //            break;

            //        case 3:
            //            FrameOfBomb++;
            //            break;

            //        case 4:
            //            FrameOfBomb++;
            //            break;

            //        case 5:
            //            FrameOfBomb++;
            //            break;

            //        case 6:
            //            FrameOfBomb++;
            //            break;

            //        case 7:
            //            FrameOfBomb++;
            //            break;

            //        case 8:
            //            FrameOfBomb++;
            //            break;

            //        case 9:
            //            FrameOfBomb++;
            //            break;

            //        case 10:
            //            Game.Components.Add(new Explosive(Game, ref texture, new Rectangle(480, 0, 32, 32), new Vector2(position.X, position.Y)));
            //            this.Dispose();
            //            break;
            //    }
            //    if (FrameOfBomb < 10)
            //        rectangle = new Rectangle(FramesOfBomb[FrameOfBomb - 1], 0, 32, 32);

            //    totalTime = 0;
            //}

            base.Update(gameTime);
        }

        void Gravity()
        {
            if (!onGround)
            {
                position.Y += gravity;
                gravity += 0.16f;
            }

            if (gravity < 0)
                while (CollideWithWall())
                {
                    gravity = 0;
                    position.Y += 1;
                }
            else
                while (CollideWithWall())
                {
                    BombGravity.Play(0.005f, 0, 0);
                    gravity = 0;
                    position.Y -= 1;
                }
        }

        bool AboveWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall) || item.GetType() == typeof(Box))
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

        bool CollideWithWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall) || item.GetType() == typeof(Box))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    { return true; }
            }
            return false;
        }
    }
}
