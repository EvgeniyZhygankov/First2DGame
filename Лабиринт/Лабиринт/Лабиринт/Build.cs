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
    public class Build : BaseClass
    {
        Hero player;
        KeyboardState kbState;
        SoundEffect soundOfDoor;
        Wall Floor;
        Portal portal;

        #region Äגונט

        Rectangle Door_Exit;
        Rectangle Door_ON_first_TO_second;
        Rectangle Door_ON_second_TO_first;

        #endregion
        
        public Build(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition, SoundEffect newSoundOfDoor, ref Texture2D newTextureForPortal ) : base(game)// ÊÎÍÑÒÐÓÊÒÎÐ
        {
            position = newPosition;
            texture = newTexture;
            rectangle = newRectangle;
            player = (Hero)Game.Services.GetService(typeof(Hero));
            portal = (Portal)Game.Services.GetService(typeof(Portal));
            Door_Exit = new Rectangle((int)position.X + 81, (int)position.Y + 49, 84, 63);
            Door_ON_first_TO_second = new Rectangle((int)position.X + 473, (int)position.Y + 49, 43, 63);
            Door_ON_second_TO_first = new Rectangle((int)position.X + 509, (int)position.Y - 73, 43, 63);
            soundOfDoor = newSoundOfDoor;
            Game.Components.Add(Floor = new Wall(Game, ref texture, new Rectangle(20, 112, 830, 10), new Vector2(20, 112) + position));
        }

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();

            if (player.inBuild)
            {
                if (player.position.X < position.X + 20)
                    player.position.X = position.X + 20;

                if (player.position.X + player.rectangle.Width > position.X + 595)
                    player.position.X = position.X + 595 - player.rectangle.Width;

                if (player.position.Y + player.rectangle.Height > rectangle.Height - 10)
                    player.position.Y = rectangle.Height - 10 - player.rectangle.Height;

                if (kbState.IsKeyDown(Keys.S) && player.DoorOpening)
                {
                    player.DoorOpening = false;

                    if (CollideWithDoor(Door_Exit))
                    {
                        player.inBuild = false;
                        player.position = new Vector2(461, 727);
                        soundOfDoor.Play(0.05f, 0, 0);
                    }

                    // ס ןונגמדמ םא געמנמי
                    if (CollideWithDoor(Door_ON_first_TO_second))
                    {
                        position.Y -= 122;
                        player.position = new Vector2(468, 49) + position;
                        rectangle = new Rectangle(0, 122, 850, 122);
                        Floor.rectangle = new Rectangle(20, 234, 630, 10);
                        Floor.position = new Vector2(20, 112) + position;
                        portal.Visible = true;
                    }

                    // סמ געמנמדמ םא ןונגûי
                    if (CollideWithDoor(Door_ON_second_TO_first))
                    {
                        position.Y += 122;
                        player.position = new Vector2(481, 49) + position;
                        rectangle = new Rectangle(0, 0, 850, 122);
                        Floor.rectangle = new Rectangle(20, 112, 810, 10);
                        Floor.position = new Vector2(20, 112) + position;
                        portal.Visible = false;
                    }
                }
                
                base.Update(gameTime);
            }
        }
            
        bool CollideWithDoor(Rectangle door)
        {
            if (player.position.X + player.rectangle.Width > door.X &&
                player.position.X < door.X + door.Width &&
                player.position.Y + player.rectangle.Height > door.Y &&
                player.position.Y < door.Y + door.Height)
                return true;
            else
                return false;
        }
    }
}
