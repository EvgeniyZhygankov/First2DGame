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
    public class Rocket : BaseClass
    {
        double time = 0;
        int orientation;

        public Rocket(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, int newOrientation) : base(game)
        {
            rectangle = newRectangle;
            position = newPosition;
            texture = newTexture;
            orientation = newOrientation;
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalSeconds;
            if (time > 0.3)
            {
                if (orientation == 1)
                    position.Y -= 3;
                else
                    position.X += 3;
            }

            if (position.Y < 0)
            {
                Game.Components.Add(new Explosive(Game, ref texture, new Rectangle(480, 0, 32, 32), new Vector2(position.X, position.Y)));
                this.Dispose();
            }

            base.Update(gameTime);
        }
    }
}
