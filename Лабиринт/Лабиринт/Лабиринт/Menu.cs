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
    public class Menu : BaseClass
    {
        static int n = 100, m = 250, smallhelp = 865;

        #region Координаты Элементов

        Vector2 smallhelpPosition = new Vector2(130, 500);
        Vector2 helpPosition = new Vector2(600, 100);
        Vector2 menuOfGamePosition = new Vector2(n, m - 200);
        Vector2 resumeGamePosition = new Vector2(n + 60, m);
        Vector2 newGamePosition = new Vector2(n, m + 75);
        Vector2 exitGameposition = new Vector2(n + 60, m + 150);
        Vector2 currentElementLeftPosition = new Vector2(n - 80, m);
        Vector2 currentElementRightPosition = new Vector2(n + 352, m);
        public Vector2 boxPosition;
        public int ChangeXOfBox = 3;
        public int ChangeYOfBox = 3;

        #endregion

        #region Прямоугольники Элементов

        Rectangle smallhelpRectangle = new Rectangle(352, smallhelp, 288, 24);
        Rectangle helpRectangle = new Rectangle(33, 486, 600, 380);
        Rectangle menuOfGameRectangle = new Rectangle(406, 937, 348, 64);
        Rectangle resumeGameRectangle = new Rectangle(0, 936, 200, 64);
        Rectangle exitGameRectangle = new Rectangle(201, 934, 204, 66); 
        Rectangle newGameRectangle = new Rectangle(0, 870, 332, 64);
        Rectangle currentElementLeftRectangle = new Rectangle(480, 0, 64, 32);
        Rectangle currentElementRightRectangle = new Rectangle(544, 0, 64, 32);
        public Rectangle boxRectangle = new Rectangle(448, 32, 64, 64);

        #endregion

        #region Указатель
        
        public int reference = 1;
        public int CountElements = 3; 

        #endregion

        Texture2D menuTexture;

        public Menu(Game game, ref Texture2D newTexture) : base(game) 
        {
            texture = newTexture;
            menuTexture = Game.Content.Load<Texture2D>("Фон для меню");
        }

        public override void Initialize() { base.Initialize(); }

        public override void Update(GameTime gameTime) { base.Update(gameTime); }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            spriteBatch.Draw(menuTexture, Vector2.Zero, Color.White); // Фон меню 
            spriteBatch.Draw(texture, menuOfGamePosition, menuOfGameRectangle, Color.White); // Меню игры
            spriteBatch.Draw(texture, exitGameposition, exitGameRectangle, Color.White); // Выход
            spriteBatch.Draw(texture, resumeGamePosition, resumeGameRectangle, Color.White); // Играть
            spriteBatch.Draw(texture, newGamePosition, newGameRectangle, Color.White); // Новая игра
            spriteBatch.Draw(texture, currentElementLeftPosition, currentElementLeftRectangle, Color.White); // Указатель на текущий элемент лево
            spriteBatch.Draw(texture, currentElementRightPosition, currentElementRightRectangle, Color.White); // Указатель на текущий элемент право
            spriteBatch.Draw(texture, helpPosition, helpRectangle, Color.White); // Справка
            spriteBatch.Draw(texture, smallhelpPosition, smallhelpRectangle, Color.White); // помощь внизу

            switch (reference)
            {
                case 1:
                    currentElementLeftPosition.Y = m + 15;
                    currentElementRightPosition.Y = m + 15;
                    smallhelpRectangle.Y = smallhelp;
                    break;

                case 2:
                    currentElementLeftPosition.Y = m + 90;
                    currentElementRightPosition.Y = m + 90;
                    smallhelpRectangle.Y = smallhelp + 24;
                    break;

                case 3:
                    currentElementLeftPosition.Y = m + 165;
                    currentElementRightPosition.Y = m + 165;
                    smallhelpRectangle.Y = smallhelp + 47;
                    break;
            }

            //base.Draw(gameTime);
        }
    }
}
