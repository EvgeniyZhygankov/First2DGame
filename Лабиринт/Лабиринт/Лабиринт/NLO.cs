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
    public class NLO : BaseClass
    {
        int random;
        
        Hero player;

        bool fire = true;

        Random rand = new Random();

        SoundEffect soundOfPlazma;

        void Motion(Vector2 speed) { position += speed; }

        public NLO(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)
        {
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
            player = (Hero)Game.Services.GetService(typeof(Hero));
            random = rand.Next(1, 4);
            soundOfPlazma = Game.Content.Load<SoundEffect>("Plazma");
        }

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime)
        {
            if (position.X + rectangle.Width + 500 < 0)
                Dispose();
            //random = 3;
            Motion(new Vector2(-20, 0));

            switch (random)
            {
                case 1:
                    if (position.X + rectangle.Width / 2 <= player.position.X && fire)
                    {
                        Game.Components.Add(new Bomb(Game, ref texture, new Rectangle(224, 0, 32, 32), new Vector2(position.X + rectangle.Width / 2, position.Y + rectangle.Height)));
                        Motion(new Vector2(-20, 0));
                        fire = false;
                    }
                    break;

                case 2:
                    if (position.X + rectangle.Width / 2 <= player.position.X && fire)
                    {
                        Game.Components.Add(new Plazma(Game, ref texture, new Rectangle(512, 33, 32, 64), new Vector2(position.X + rectangle.Width / 2, position.Y + rectangle.Height), new Vector2(0, 10)));
                        soundOfPlazma.Play(0.005f, 0, 0);
                        Motion(new Vector2(-10, 0));
                        fire = false;
                    }
                    break;

                case 3:
                    if (position.X + rectangle.Width / 2 <= player.position.X && fire)
                    {
                        Game.Components.Add(new Plazma(Game, ref texture, new Rectangle(288, 0, 32, 32), new Vector2(position.X + rectangle.Width / 2, position.Y + rectangle.Height), new Vector2(0, 2)));
                        Motion(new Vector2(-20, 0));
                        fire = false;
                    }
                    break;
            }
            

            base.Update(gameTime);
        }
    }
}
