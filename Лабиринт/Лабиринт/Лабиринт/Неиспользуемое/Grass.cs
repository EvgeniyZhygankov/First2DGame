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
    public class Grass : BaseClass
    {
        double TimeOfGrass;

        public Grass(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
        }

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime) 
        {
            if (CollideWithGrass())
            {
                rectangle = new Rectangle(832, 0, 32, 32);

            }
            else
            {
                TimeOfGrass += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TimeOfGrass > 4500)
                {
                    rectangle = new Rectangle(800, 0, 32, 32);
                    TimeOfGrass = 0;
                }
            }

            base.Update(gameTime); 
        }

        bool CollideWithGrass()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Hero) || item.GetType() == typeof(Box))
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
