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
    public class Portal : BaseClass
    {
        float scale = 1f, rotation = 0;
        bool end = false;
        float downBorder, upBorder;

        public Portal(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
            downBorder = newPosition.Y + 20;
            upBorder = newPosition.Y;
        }
        
        public override void Initialize() { base.Initialize(); }
        
        public override void Update(GameTime gameTime) // UPDATE
        {
            if (!end)
                if (position.Y < downBorder)
                    position.Y += 0.3f;
                else
                    end = true;
            else
                if (position.Y > upBorder)
                    position.Y -= 0.3f;
                else
                    end = false;

            rotation -= 0.03f;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            
            spriteBatch.Draw(texture, position + new Vector2(6, 6), new Rectangle(13, 0, 71, 71), Color.White, rotation, new Vector2(37, 35), scale, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, position + new Vector2(6, 6), new Rectangle(84, 0, 71, 71), Color.White, -rotation, new Vector2(35, 35), scale, SpriteEffects.None, 0);

            base.Draw(gameTime);
        }
    }
}
