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
using System.Threading;


namespace MakedAtNight
{
    public class Hero : BaseClass 
    {
        KeyboardState kbState;

        Texture2D textureGGHD;

        int speed;
        float gravity = 0;
        public bool onGround, onLadder, jumped = true;

        #region Работа с frame'ами

        int HorizontalFrame = 0, VerticalFrame = 0;
        float TimeForVerticalFrame = 300, TimeForHorizontalFrame = 200;
        double TotalTime;

        #endregion

        // Плазма
        bool fire = true;

        // Здоровье игрока
        public int HelthPoints = 7;
        public int Ammo = 0;
        bool pain = true;

        #region Работа со звуом

        SoundEffect ladderSound;
        SoundEffect wallSound;
        SoundEffect plazmaSound;
        public SoundEffect painSound;

        int ChekSound = 1;

        float volume = 0.05f;
        #endregion

        #region Работа с камерой

        Vector3 camera;
        public int VerticalChekCamera = 704;
        public int HorizontalChekCamera = 500;

        #endregion

        #region Работа со светом

        public Rectangle rectangleOfLight = new Rectangle(0, 160, 200, 200);
        public Vector2 positionOfLight = new Vector2(-200, -200);

        #endregion

        #region Работа с текстурой получения урона 

        Texture2D textureOfDamage;
        bool getDamage = false;
        double timeForTextureOfDamage = 0;

        #endregion

        // Ракета
        //bool Rocket = true;

        // Бомба          
        //bool BOOM = true;

        // Работа с досками
        //double PoPrikoly;

        // Работа с цепью
        //bool chain = true;

        public bool inBuild = false, DoorOpening = true;

        public Vector3 Camera() { return camera; }

        void Motion(Vector2 speed) { position += speed; }

        void MotionCamera(Vector3 speed) { camera += speed; }

        void PressUp(GameTime gameTime)
        {
            if (CollideWithSomthing(typeof(Ladder)) != null && CollideWithSomthing(typeof(Wall)) == null)
            {
                int[] framesLadder = { 34, 66 };
                TotalTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TotalTime > TimeForVerticalFrame)
                {
                    ladderSound.Play(volume, 0, 0);

                    if (VerticalFrame < 2)
                        VerticalFrame++;
                    else
                        VerticalFrame = 1;

                    rectangle = new Rectangle(framesLadder[VerticalFrame - 1], 32, 28, 64);
                    TotalTime = 0;
                }
                Motion(new Vector2(0, -2));
            }

            while (CollideWithSomthing(typeof(Wall)) != null)
            { Motion(new Vector2(0, 1)); }
        }

        void PressDown(GameTime gameTime)
        {
            if (CollideWithSomthing(typeof(Ladder)) != null && !AboveWall() || AboveLadder() && !AboveWall())
            {
                int[] framesLadder = { 34, 66 };
                TotalTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TotalTime > TimeForVerticalFrame)
                {
                    ladderSound.Play(volume, 0, 0);
                    
                    if (VerticalFrame < 2)
                        VerticalFrame++;
                    else
                        VerticalFrame = 1;

                    rectangle = new Rectangle(framesLadder[VerticalFrame - 1], 32, 28, 64);
                    TotalTime = 0;
                }
                Motion(new Vector2(0, 2));
            }            

            while (CollideWithSomthing(typeof(Wall)) != null)
            { Motion(new Vector2(0, -1)); }
        }

        void PressRight(GameTime gameTime)
        {
            if (CollideWithSomthing(typeof(Ladder)) == null && onGround || AboveWall())
            {
                int[] frames = {128, 160, 192, 224};
                TotalTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TotalTime > TimeForHorizontalFrame)
                {
                    if (ChekSound < 0)
                    {
                        ChekSound -= 2 * ChekSound;
                        wallSound.Play(volume, 0, 0);
                    }
                    else
                        ChekSound -= 2 * ChekSound;

                    if (HorizontalFrame < 4)
                        HorizontalFrame++;
                    else
                        HorizontalFrame = 1;

                    rectangle = new Rectangle(frames[HorizontalFrame - 1], 33, 32, 63);
                    TotalTime = 0;
                }
            }
            
            Motion(new Vector2(speed, 0));
            

            while (CollideWithSomthing(typeof(Wall)) != null)
            { Motion(new Vector2(-1, 0)); }
        }

        void PressLeft(GameTime gameTime)
        {
            if (CollideWithSomthing(typeof(Ladder)) == null && onGround || AboveWall())
            {
                int[] frames = { 352, 320, 288, 256 };
                TotalTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TotalTime > TimeForHorizontalFrame)
                {
                    if (ChekSound < 0)
                    {
                        ChekSound -= 2 * ChekSound;
                        wallSound.Play(volume, 0, 0);
                    }
                    else
                        ChekSound -= 2 * ChekSound;

                    if (HorizontalFrame < 4)
                        HorizontalFrame++;
                    else
                        HorizontalFrame = 1;

                    rectangle = new Rectangle(frames[HorizontalFrame - 1], 33, 32, 63);
                    TotalTime = 0;
                }
            }

            Motion(new Vector2(-speed, 0));

            while (CollideWithSomthing(typeof(Wall)) != null)
            { Motion(new Vector2(1, 0)); }
        }

        public Hero(Game game, ref Texture2D newTexture, Rectangle newRectangle, Vector2 newPosition) : base(game)// КОНСТРУКТОР
        {
            textureGGHD = Game.Content.Load<Texture2D>("ГГБ");
            texture = newTexture;
            position = newPosition;
            rectangle = newRectangle;
            ladderSound = Game.Content.Load<SoundEffect>("Ladder2");
            wallSound = Game.Content.Load<SoundEffect>("Step2");
            plazmaSound = Game.Content.Load<SoundEffect>("Plazma");
            painSound = Game.Content.Load<SoundEffect>("fallpain");
            camera = new Vector3(0, -100, 0);
            textureOfDamage = Game.Content.Load<Texture2D>("Урон");
        } 

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime) // UPDATE
        {
            onGround = false;
            onLadder = false;
            speed = 3;
            kbState = Keyboard.GetState();
            AboveWall();
            AboveLadder();
            GravityAndJump(kbState);

            if (kbState.IsKeyUp(Keys.S))
                DoorOpening = true;

            #region Методы Collide

            if (CollideWithSomthing(typeof(Portal)) != null)
            {
                // Ебать, что за говно ты тут толкал, надо было переменной inBuild поставить "false" типо не в здании, 
                // а то проверка по координатом возвращала true, так как пока в здании, то за пределы его ты не можешь выйти.
                // фикс от: 29.10.2016 16:24 (пока в поезде ехал);
                inBuild = false;
                position = new Vector2(-5000, -700);
            }



            if (CollideWithSomthing(typeof(Packs)) != null)
            {
                Ammo += 40;
                CollideWithSomthing(typeof(Packs)).Dispose();
            }



            if (HelthPoints <= 0)
                Dispose();



            if (CollideWithSomthing(typeof(Plazma)) != null || (CollideWithSomthing(typeof(Alien)) != null || CollideWithSomthing(typeof(Explosive)) != null) && pain)
            {
                if (CollideWithSomthing(typeof(Plazma)) != null)
                    CollideWithSomthing(typeof(Plazma)).Dispose();

                if (CollideWithSomthing(typeof(Alien)) != null || CollideWithSomthing(typeof(Explosive)) != null)
                    pain = false;

                Damage();
                getDamage = true;
            }

            if (CollideWithSomthing(typeof(Alien)) == null && CollideWithSomthing(typeof(Explosive)) == null)
                pain = true;

            #endregion
            
            #region Фонарь

            if (position.Y > 800)
                positionOfLight = new Vector2(position.X - 80, position.Y - 100);

            if (position.Y < 800)
                positionOfLight = new Vector2(-200, -200);

            #endregion

            #region Камера

            while(position.Y + rectangle.Height > VerticalChekCamera)
            {
                VerticalChekCamera += 1;
                MotionCamera(new Vector3(0, -1, 0));
            }

            while(position.Y < VerticalChekCamera - 160)
            {
                VerticalChekCamera -= 1;
                MotionCamera(new Vector3(0, 1, 0));
            }
            
            while(position.X + rectangle.Width > HorizontalChekCamera)
            {
                HorizontalChekCamera += 1;
                MotionCamera(new Vector3(-1, 0, 0));
            }

            while(position.X < HorizontalChekCamera)
            {             
                HorizontalChekCamera -= 1;
                MotionCamera(new Vector3(1, 0, 0));
            }

            #endregion

            #region Выстрел

            if (Ammo > 80)
                Ammo = 80;

            if (Ammo > 0)
            {
                if (kbState.IsKeyDown(Keys.NumPad4) && fire && !onLadder)
                {
                    Ammo--;
                    Game.Components.Add(new Plazma(Game, ref texture, new Rectangle(640, 32, 16, 6), new Vector2(position.X - 40, position.Y + 16), new Vector2(-5, 0)));
                    fire = false;
                }

                if (kbState.IsKeyDown(Keys.NumPad6) && fire && !onLadder)
                {
                    Ammo--;
                    Game.Components.Add(new Plazma(Game, ref texture, new Rectangle(640, 32, 16, 6), new Vector2(position.X + 40, position.Y + 16), new Vector2(5, 0)));
                    fire = false;
                }

            }

            if (kbState.IsKeyUp(Keys.NumPad4) && kbState.IsKeyUp(Keys.NumPad6))
                fire = true;

            #endregion

            #region Анимация передвижения

            //if (onLadder)
            //    rectangle = new Rectangle(34, 32, 28, 64);

            // Прыжок вправо
            if (kbState.IsKeyDown(Keys.D) && !onGround && !onLadder)
                rectangle = new Rectangle(416, 33, 32, 63);

            // Прыжок влево
            if (kbState.IsKeyDown(Keys.A) && !onGround && !onLadder)
                rectangle = new Rectangle(384, 33, 32, 63);
            
            // Падение по прямой
            if (!onGround && !onLadder && jumped && kbState.IsKeyUp(Keys.A) && kbState.IsKeyUp(Keys.D))
            { rectangle = new Rectangle(98, 33, 32, 63); }

            // Стоячее положение
            if (onGround && kbState.IsKeyUp(Keys.D) && kbState.IsKeyUp(Keys.A) ||
                AboveLadder() && CollideWithSomthing(typeof(Ladder)) == null && !kbState.IsKeyDown(Keys.D) && !kbState.IsKeyDown(Keys.A))
            { rectangle = new Rectangle(2, 33, 28, 63); }

            #endregion
            
            #region Передвижение

            // На случай если надумаешь вернуть цепь, то к каждому передвижению приклей условие, чтобы была отпущена кнопка "E"
            if (kbState.IsKeyDown(Keys.W) && kbState.IsKeyUp(Keys.LeftControl))
            { PressUp(gameTime); }

            if (kbState.IsKeyDown(Keys.S))
            { PressDown(gameTime); }

            if (kbState.IsKeyDown(Keys.A))
            { PressLeft(gameTime); }

            if (kbState.IsKeyDown(Keys.D))
            { PressRight(gameTime); }

            if (kbState.IsKeyUp(Keys.W))
                jumped = true;

            #endregion

            #region Мусор

            //if (CollideWithAlien() != null && pain)
            //{
            //    pain = false;
            //    getDamage = true;
            //}

            //if (CollideWithSome(typeof(Rubbish)) != null)
            //CollideWithSome(typeof(Rubbish)).Dispose();

            // Мины
            //if (kbState.IsKeyDown(Keys.NumPad5) && BOOM)
            //{
            //    Game.Components.Add(new Bomb(Game, ref texture, new Rectangle(192, 0, 32, 32), new Vector2(position.X, position.Y + 32)));
            //    BOOM = false;
            //}

            //if (kbState.IsKeyUp(Keys.NumPad5))
            //    BOOM = true;

            // Кошка и цепь
            //while (kbState.IsKeyDown(Keys.E) && chain && !RightOrLeftWall())
            //{
            //    rectangle = new Rectangle(); // Здесь могли бы быть ваши координаты анимации взаимодействия игрока с цепью :D уже есть ебаный врот
            //    Game.Components.Add(new Chain(Game, ref texture, new Rectangle(728, 64, 20, 8), new Vector2(position.X + rectangle.Width, position.Y + 16)));
            //    chain = false;
            //}

            //if (kbState.IsKeyUp(Keys.E))
            //    chain = true;

            // Работа с ракетами
            // Горизонтальная
            //if (kbState.IsKeyDown(Keys.F) && Rocket)
            //{
            //    Game.Components.Add(new Rocket(Game, ref texture, new Rectangle(608, 0, 136, 32), new Vector2(position.X + 32, position.Y), 2));
            //    Rocket = false;
            //}

            // Вертикальная
            //if (kbState.IsKeyDown(Keys.Q) && Rocket) 
            //{     
            //    Game.Components.Add(new Rocket(Game, ref texture, new Rectangle(576, 0, 32, 136), new Vector2(position.X + 32, position.Y - 73), 1));
            //    Rocket = false;
            //}

            //if (kbState.IsKeyUp(Keys.Q) && kbState.IsKeyUp(Keys.F))
            //    Rocket = true;

            // Пропадающие доски
            //if (AboveWall1() != null && AboveWall1().rectangle.Height == 5)
            //{
            //    PoPrikoly += gameTime.ElapsedGameTime.TotalSeconds;
            //    if (PoPrikoly > 0.15)
            //    {
            //        AboveWall1().Dispose();
            //        PoPrikoly = 0;
            //    }
            //}

            #endregion

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            spriteBatch.Draw(texture, position, new Rectangle(0, 0, 82, 191), Color.White, 0, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            // timeForTextureOfDamage - время на появление экрана, который символизирует о получении урона.
            // getDamage - флаг о получении урона в данный момент (получили и все, больше не получаем).
            if (getDamage)
            {
                timeForTextureOfDamage += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeForTextureOfDamage < 200)
                {
                    spriteBatch.Draw(textureOfDamage, new Vector2(-camera.X, VerticalChekCamera - 605), Color.White);
                }   
                else
                {
                    timeForTextureOfDamage = 0;
                    getDamage = false;
                }                 
            }
            

            base.Draw(gameTime);
        }

        void GravityAndJump(KeyboardState state)
        {
            if (state.IsKeyDown(Keys.W) && onGround && CollideWithSomthing(typeof(Ladder)) == null && !AboveLadder() && jumped && !inBuild)
            {
                jumped = false;
                onGround = false;
                gravity = -5f;
            }

            if (!onGround && !onLadder)
            { gravity += 0.16f; }

            if (onLadder)
                gravity = 0;

            Motion(new Vector2(0, gravity));

            camera.Y = (int)camera.Y;
            position.Y = (int)position.Y;

            if (gravity < 0)
                while (CollideWithSomthing(typeof(Wall)) != null || CollideWithSomthing(typeof(Box)) != null)
                {
                    gravity = 0;
                    Motion(new Vector2(0, 1));
                }
            else
                while (CollideWithSomthing(typeof(Wall)) != null || CollideWithSomthing(typeof(Box)) != null)
                {
                    if (gravity > 8)
                    {
                        Damage();
                        getDamage = true;
                    }
                    gravity = 0;
                    Motion(new Vector2(0, -1));
                }
        }

        void Damage()
        {
            painSound.Play(volume, 0, 0);
            HelthPoints--;
        }

        bool AboveWall()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Wall) || item.GetType() == typeof(Box))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + 1 + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    {
                        gravity = 0;
                        onGround = true;
                        return true;
                    }       
            }
            return false;
        }

        bool AboveLadder()
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == typeof(Ladder))
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + 1 + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    {
                        onLadder = true;
                        return true;
                    }            
            }
            return false;
        }

        // Проверки:
        //1) Справа и лева есть-ли стена 
        //2) Проверка на столкновение со стеной, которое возвращает тип объекта (хз зачем было нужно, так и не нашел ее вызов)

        //bool RightOrLeftWall(int n)
        //{
        //    foreach (BaseClass wall in Game.Components)
        //    {
        //        if (wall.GetType() == typeof(Wall))
        //            if (position.X + rectangle.Width + n > wall.position.X &&
        //                position.X - n < wall.position.X + wall.rectangle.Width &&
        //                position.Y + rectangle.Height > wall.position.Y &&
        //                position.Y < wall.position.Y + wall.rectangle.Height)
        //            {
        //                return true;
        //            }

        //    }
        //    return false;
        //}

        //BaseClass AboveWall1()
        //{
        //    foreach (BaseClass wall in Game.Components)
        //    {
        //        if (wall.GetType() == typeof(Wall))
        //            if (position.X + rectangle.Width > wall.position.X &&
        //                position.X < wall.position.X + wall.rectangle.Width &&
        //                position.Y + 1 + rectangle.Height > wall.position.Y &&
        //                position.Y < wall.position.Y + wall.rectangle.Height)
        //            {
        //                gravity = 0;
        //                onGround = true;
        //                return wall;
        //            }
        //    }
        //    return null;
        //}

        BaseClass CollideWithSomthing(Type qwe)
        {
            foreach (BaseClass item in Game.Components)
            {
                if (item.GetType() == qwe)
                    if (position.X + rectangle.Width > item.position.X &&
                        position.X < item.position.X + item.rectangle.Width &&
                        position.Y + rectangle.Height > item.position.Y &&
                        position.Y < item.position.Y + item.rectangle.Height)
                    { return item; }
            }
            return null;
        }
    }
}