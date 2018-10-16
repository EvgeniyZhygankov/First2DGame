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

    public class Explosive : BaseClass
    {
        double time = 0;
        Vector2 pos;
        SoundEffect Explode;

        public Explosive(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            rectangle = newRectangle;
            position = newPosition;
            texture = newTexture;
            pos = position;
            Explode = Game.Content.Load<SoundEffect>("Explode");
        }

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalSeconds;
            if (time > 0.3)
            {
                position = new Vector2(pos.X - 16, pos.Y - 16);
                rectangle = new Rectangle(576, 32, 64, 64);
            }

            if (time > 0.5)
            {
                Explode.Play(0.002f, 0, 0);
                this.Dispose();
            }

            base.Update(gameTime);
        }
    }
}
