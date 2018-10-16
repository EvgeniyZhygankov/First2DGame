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
    public class Plazma : BaseClass
    {
        Vector2 speed;

        public Plazma(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, Vector2 newSpeed) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
            speed = newSpeed;            
        }

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime)
        {
            position += speed;

            if (CollideWithWall())
                this.Dispose();

            base.Update(gameTime);
        }

        bool CollideWithWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall))
                {
                    if (position.X + rectangle.Width > item.position.X &&
                    position.X < item.position.X + item.rectangle.Width &&
                    position.Y + rectangle.Height > item.position.Y &&
                    position.Y < item.position.Y + item.rectangle.Height)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
