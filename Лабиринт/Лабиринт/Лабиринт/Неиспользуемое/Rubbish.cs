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
    public class Rubbish : BaseClass
    {
        int time = 0, chek = -1; 
        
        float gravity = 0;

        bool onGround;

        void Motion(Vector2 speed) { position += speed; }

        public Rubbish(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
        }

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime)
        {
            onGround = false;
            Gravity();

            time += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (time >= 1000)
            {
                time = 0;
                chek -= chek * 2;
            }

            if (chek > 0)
                rectangle = new Rectangle(160, 0, 32, 32);
            else
                rectangle = new Rectangle(192, 0, 32, 32);

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
    }
}
