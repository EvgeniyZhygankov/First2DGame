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
    public class BaseClass : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Texture2D texture;
        public Rectangle rectangle;
        public Vector2 position;
       
        public BaseClass(Game game) : base(game){}

        public override void Initialize() { base.Initialize(); }
        
        public override void Update(GameTime gameTime) { base.Update(gameTime); }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            spriteBatch.Draw(texture, position, rectangle, Color.White);

            base.Draw(gameTime);
        }
    }
}
