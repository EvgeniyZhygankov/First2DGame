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
    public class Ladder : BaseClass
    {
        public Ladder(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPositon)
            : base(game)
        {
            texture = newTexture;
            rectangle = newRectangle;
            position = newPositon;
        }

        public override void Initialize()
        { base.Initialize(); }

        public override void Update(GameTime gameTime)
        { base.Update(gameTime); }
    }
}
