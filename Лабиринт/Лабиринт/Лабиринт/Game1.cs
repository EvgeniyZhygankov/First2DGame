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
                // Буду вести небольшой учет изменений, интересно же :D
                // 17.09.2016 16:41 (уже хз что там было), вроде обновил построение карты
                // 29.10.2016 16:24 (пофикшен бак с проваливающимся игроком при заходе в телепорт)
                // 04.11.2016 13:02 (ввел остров, пока еще не сделал провалы в асфальте)
                // 24.11.2016 14:31 просто зашел в игру и сразу вышел, хз че сделать, на физре сижу, стола нет и мышку не на что поставить чтобы в фотошопе нарисовать что-либо.
                // 12.12.2016 12:30 убрал две строчки в классе Friend путем сокращения кода в методе ThirtyCommand(), там вызвал метод первой команды, а потом изменил кое-что.
                // 01.04.2017 14:47 Попробую протестить масштабирование спрайтов. Не в этом проекте. 16:10 Получилось - параметр scale у Draw.
                // 09.05.2017 15:30 МБ создать класс, с методами на проверку пересечения с различными объектами.
                // 05.08.2017 16:38 
                // 21.08.2017 23:10 

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Texture2D texture;
        Texture2D sky;
        Texture2D sky2;
        Texture2D intereface;
        Texture2D textureForPortal;
        Texture2D island;

        Matrix Camera;
        Hero player;
        Friend helper;
        public Portal portal;

        double timeOfAlien = 0, timeOfNLO = 0, timeOfPack = 0, timeToNewGame = 0;

        #region Работа с интерфейсом

        Vector2 interefacePosition;
        SpriteFont spriteFont;
        SpriteFont spriteFont2;
        Vector2 modeOfHelperPosition;

        #endregion

        #region Работа с фонариком

        BlendState blendState;
        RenderTarget2D foreground;
        RenderTarget2D Shadows;

        #endregion

        #region  Работа с меню

        Menu menu;
        KeyboardState kbState;

        bool paused = true; 
        bool pauseKeyDown = false;
        bool Opportunity = true;

        SoundEffect Switch;

        #endregion

        #region Здание

        Build build;
        Texture2D textureOfBuild;
        Rectangle rectangleOfDoor = new Rectangle(1020 - 588, 659 + 8, 84, 77);
        SoundEffect soundOfDoor;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 736;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            texture = Content.Load<Texture2D>("Текстуры");
            sky = Content.Load<Texture2D>("День");
            sky2 = Content.Load<Texture2D>("День2");
            textureOfBuild = Content.Load<Texture2D>("Здание");
            intereface = Content.Load<Texture2D>("Интерфейс");
            textureForPortal = Content.Load<Texture2D>("Портал");
            island = Content.Load<Texture2D>("Острова");

            spriteFont = Content.Load<SpriteFont>("Шрифт для ХП и Патрон");
            spriteFont2 = Content.Load<SpriteFont>("Нет патрон");

            menu = new Menu(this, ref texture);

            Switch = Content.Load<SoundEffect>("Switch");
            soundOfDoor = Content.Load<SoundEffect>("Door");

            // новая технология построения карты. внедрена 17.08.2016 (дополнена 17.09.2016)
            NewGame();
        }

        protected override void UnloadContent(){}

        void Pause()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                pauseKeyDown = true;
            else 
                if (pauseKeyDown == true)
                {
                    pauseKeyDown = false;
                    paused = !paused;
                }
        }

        void NewGame()
        {
            GraphicsDevice.Clear(Color.Black);
            Components.Clear();
            Services.RemoveService(typeof(Hero));
            Services.RemoveService(typeof(Portal));

            Components.Add(new Wall(this, ref sky, new Rectangle(587, 758, 1275, 1), new Vector2(5, 790)));
            Components.Add(new Wall(this, ref sky, new Rectangle(587, 758, 1275, 1), new Vector2(1275, 790)));

            Components.Add(player = new Hero(this, ref texture, new Rectangle(2, 33, 28, 63), new Vector2(35, 736)));
            Components.Add(helper = new Friend(this, ref texture, new Rectangle(2, 96, 28, 63), new Vector2(200, 736)));
            Services.AddService(typeof(Hero), player);

            portal = new Portal(this, ref textureForPortal, new Rectangle(0, 0, 13, 13), new Vector2(-4900, -80));
            portal.Visible = false;
            Services.AddService(typeof(Portal), portal);

            Components.Add(build = new Build(this, ref textureOfBuild, new Rectangle(0, 0, 850, 122), new Vector2(-5000, 0), soundOfDoor, ref textureForPortal));
            Components.Add(portal);

            Components.Add(new Island(this, ref island, new Rectangle(0, 0, 468, 1000), new Vector2(-5000, -1000)));
        }

        protected override void Update(GameTime gameTime)
        {
            kbState = Keyboard.GetState();

            if (!paused)
            {
                Pause();

                if (player.position.X + player.rectangle.Width > rectangleOfDoor.X &&
                    player.position.X < rectangleOfDoor.X + rectangleOfDoor.Width &&
                    player.position.Y + player.rectangle.Height > rectangleOfDoor.Y &&
                    player.position.Y < rectangleOfDoor.Y + rectangleOfDoor.Height && kbState.IsKeyDown(Keys.S) && player.DoorOpening)
                {
                    player.DoorOpening = false;
                    player.inBuild = true;
                    player.position = new Vector2(108, 49) + build.position;
                    soundOfDoor.Play(0.05f, 0, 0);
                }



                if (player.HelthPoints <= 0)
                {
                    timeToNewGame += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (timeToNewGame > 5000)
                    {
                        timeToNewGame = 0;
                        paused = true;
                    }
                }



                if (!player.inBuild)
                {
                    timeOfPack += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (timeOfPack > 8000)
                    {
                        //Components.Add(new Packs(this, ref texture, new Rectangle(608, 0, 32, 32), new Vector2(200, 300)));
                        timeOfPack = 0;
                    }

                    timeOfNLO += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (timeOfNLO > 4000)
                    {
                        //Components.Add(new NLO(this, ref texture, new Rectangle(0, 160, 178, 80), new Vector2(3000, 300)));
                        //Components.Add(new NLO(this, ref texture, new Rectangle(0, 160, 178, 80), new Vector2(3400, 200)));
                        timeOfNLO = 0;
                    }

                    timeOfAlien += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (timeOfAlien > 9000)
                    {
                        //Components.Add(new Alien(this, ref texture, new Rectangle(0, 256, 32, 64), new Vector2(800, 576)));
                        //Components.Add(new Alien(this, ref texture, new Rectangle(0, 256, 32, 64), new Vector2(864, 576)));
                        //Components.Add(new Alien(this, ref texture, new Rectangle(0, 256, 32, 64), new Vector2(928, 576)));
                        timeOfAlien = 0;
                    }
                }

                if (player.position.X < 0 && player.position.Y > 500)
                    player.position.X = 0;

                if (player.position.X + player.rectangle.Width > 2550 && player.position.Y > 500)
                    player.position.X = 2550 - player.rectangle.Width;

                foreach (BaseClass item in Components)
                {
                    if (item.GetType() == typeof(Box))
                    {
                        if (item.position.X < 32)
                            item.position.X = 32;

                        if (item.position.X + item.rectangle.Width > 2560)
                            item.position.X = 2496;
                    }
                }

                base.Update(gameTime);
                Camera = Matrix.CreateTranslation(player.Camera());
            }
            else
            {
                #region Коробка в меню

                //if (menu.boxPosition.X < 50)
                //{
                //    menu.ChangeXOfBox -= 2 * menu.ChangeXOfBox;
                //    menu.boxPosition.X = 50;
                //}

                //if (menu.boxPosition.X > graphics.PreferredBackBufferWidth - menu.boxRectangle.Width)
                //{
                //    menu.ChangeXOfBox -= 2 * menu.ChangeXOfBox;
                //    menu.boxPosition.X = graphics.PreferredBackBufferWidth - menu.boxRectangle.Width;
                //}

                //if (menu.boxPosition.Y < 50)
                //{
                //    menu.ChangeYOfBox -= 2 * menu.ChangeYOfBox;
                //    menu.boxPosition.Y = 50;
                //}

                //if (menu.boxPosition.Y > graphics.PreferredBackBufferHeight - menu.boxRectangle.Height)
                //{
                //    menu.ChangeYOfBox -= 2 * menu.ChangeYOfBox;
                //    menu.boxPosition.Y = graphics.PreferredBackBufferHeight - menu.boxRectangle.Height;
                //}

                #endregion

                if (kbState.IsKeyDown(Keys.W) && Opportunity)
                {
                    Switch.Play(0.05f, 0, 0);
                    if (menu.reference > 1)
                        menu.reference--;
                    else
                        menu.reference = menu.CountElements;

                    Opportunity = false;
                }

                if (kbState.IsKeyDown(Keys.S) && Opportunity)
                {
                    Switch.Play(0.05f, 0, 0);
                    if (menu.reference < menu.CountElements)
                        menu.reference++;
                    else
                        menu.reference = 1;

                    Opportunity = false;
                }

                if (kbState.IsKeyUp(Keys.W) && kbState.IsKeyUp(Keys.S))
                    Opportunity = true;

                if (kbState.IsKeyDown(Keys.Enter))
                {
                    switch(menu.reference)
                    {
                        case 1:
                            paused = false;
                            break;

                        case 2:
                            NewGame();
                            paused = false;
                            break;

                        case 3:
                            Exit();
                            break;
                    }
                }
                menu.Update(gameTime);
            }            
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!paused)
            {
                GraphicsDevice.Clear(Color.Black);

                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Camera);

                spriteBatch.Draw(sky, new Vector2(-582, 32), Color.White);
                spriteBatch.Draw(sky2, new Vector2(1280, 32), Color.White);


                base.Draw(gameTime);
                helper.Draw(gameTime);
                //player.Draw(gameTime);

                #region Работа с интерфейсом

                //spriteBatch.DrawString(spriteFont2, "X - " + portal.position.X + "Y - " + portal.position.Y, portal.position - new Vector2(0, 0), Color.Red); // координаты портала
                //spriteBatch.DrawString(spriteFont2, "X - " + player.position.X + "Y - " + player.position.Y, player.position - new Vector2(0, 0), Color.Red); // координаты игрока

                interefacePosition = new Vector2(-player.Camera().X, player.VerticalChekCamera + 32); // снизу интерфейс
                spriteBatch.Draw(intereface, interefacePosition, Color.White);

                //interefacePosition = new Vector2(-player.Camera().X, player.VerticalChekCamera - 604); // сверху интерфейс
                //spriteBatch.Draw(intereface, interefacePosition, Color.White);

                if (helper != null)
                {
                    if (!helper.secondCommand && !helper.thirtyCommand)
                        modeOfHelperPosition = new Vector2(interefacePosition.X + 250, interefacePosition.Y);

                    if (helper.secondCommand)
                        modeOfHelperPosition = new Vector2(interefacePosition.X + 250, interefacePosition.Y + 35);

                    if (helper.thirtyCommand)
                        modeOfHelperPosition = new Vector2(interefacePosition.X + 250, interefacePosition.Y + 70);

                    spriteBatch.Draw(texture, modeOfHelperPosition, new Rectangle(672, 0, 30, 30), Color.White);
                }

                spriteBatch.DrawString(spriteFont2, "Нажмите Esc - для выхода в меню игры", new Vector2(-player.Camera().X, player.VerticalChekCamera - 600), Color.Red);

                //spriteBatch.Draw(texture, new Vector2(interefacePosition.X + 105, interefacePosition.Y + 34), new Rectangle(612, 3, 6, 16), Color.White); // Патроны значок
                //spriteBatch.Draw(texture, new Vector2(interefacePosition.X + 73, interefacePosition.Y + 10), new Rectangle(640, 44, 16, 16), Color.White); // Жизни значок

                spriteBatch.DrawString(spriteFont, player.HelthPoints + "", new Vector2(interefacePosition.X + 32, interefacePosition.Y + 3), Color.Pink); // Жизни цифра

                if (player.Ammo > 0)
                    spriteBatch.DrawString(spriteFont, player.Ammo + "", new Vector2(interefacePosition.X + 32, interefacePosition.Y + 35), Color.Pink); // Патроны цифра
                else
                    spriteBatch.DrawString(spriteFont2, "Нет патрон", new Vector2(interefacePosition.X + 35, interefacePosition.Y + 37), Color.Red);


                //spriteBatch.DrawString(spriteFont, "", new Vector2(interefacePosition.X + 32, interefacePosition.Y + 70), Color.Pink); // Броня цифра

                //if (HelthOfBoss > 0)
                    //spriteBatch.Draw(texture, new Vector2(800, 400), new Rectangle(33, 357, 293, 24), Color.White); // Убейте его


                if (player.HelthPoints <= 0)
                {
                    Components.Clear();
                    GraphicsDevice.Clear(Color.Black);
                    //if (player.HelthPoints <= 0 && HelthOfBoss > 0)
                    spriteBatch.Draw(texture, new Vector2(-player.Camera().X + 400, player.VerticalChekCamera - 300), new Rectangle(453, 385, 438, 95), Color.White); // если проиграл
                        //spriteBatch.Draw(texture, new Vector2(-player.Camera().X + 400, player.VerticalChekCamera - 300), new Rectangle(33, 381, 420, 99), Color.White); если выйграл
                }

                #endregion
                
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();

                menu.Draw(gameTime);

                spriteBatch.End();
            }
            
        }
    }
}