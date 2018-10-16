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
    public class Chain : BaseClass
    {
        int speedOfChain = 5;

        public Chain(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
        }

        public override void Initialize()
        { base.Initialize(); }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                if (!CollideWithWall() && rectangle.Width < 140)
                { rectangle = new Rectangle(rectangle.X - speedOfChain, 64, rectangle.Width + speedOfChain, 8); }
                else
                {
                    foreach (BaseClass item in Game.Components)
                    {
                        if (item.GetType() == typeof(Hero))
                        {
                            item.rectangle = new Rectangle(1, 96, 63, 32);
                            item.position = new Vector2(position.X + rectangle.Width - 63, position.Y); // БАГ - какая скорость - такое и отступление :D, вроде решено
                            this.Visible = false;
                        }
                    }
                }

                if (rectangle.Width == 140)
                { this.Dispose(); }
            }
            else
            { this.Dispose(); }

            base.Update(gameTime);
        }

        bool CollideWithWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall))
                {
                    if (position.X + rectangle.Width + 1 > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    { return true; }
                }
            }
            return false;
        }
    }
}
