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
    public class Island : BaseClass
    {

        public Island(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
            Game.Components.Add(new Wall(Game, ref texture, new Rectangle(0, 415, 468, 10), new Vector2(0, 415) + position));
        }

        public override void Initialize() { base.Initialize(); }
        
        public override void Update(GameTime gameTime) // UPDATE
        {

            base.Update(gameTime);
        }
    }
}
