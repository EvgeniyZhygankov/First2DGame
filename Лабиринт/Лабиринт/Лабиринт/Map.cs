using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MakedAtNight
{
    public class Map : BaseClass
    {
        public string pathToBackGround = "Карта_Задний слой.csv";
        public string pathToMap = "Карта_Почти все текстуры.csv";
        public string pathToBoxs = "Карта_Коробки.csv";
        public string pathToEnemes = "Карта_Враги.csv";

        const int n = 30;
        const int m = 80;

        int[,] level = new int[n, m];

        public Map(Game game, ref Texture2D newTexture) : base(game) { texture = newTexture; }

        public void GenerateMap(string path)
        {
            //string[] ferm = File.ReadAllLines("Карта 1_Тест.csv");
            string[] ferm = File.ReadAllLines(path);
            string str = "";
            int q = 0, w = 0;

            for (int i = 0; i <= ferm.Length - 1; i++)
            {
                if (i > 0)
                {
                    level[q, w] = Int32.Parse(str);
                    str = "";
                    if (w < m - 1)
                        w++;
                    else
                    {
                        w = 0;
                        if (q < n - 1)
                            q++;
                    }
                }

                for (int j = 0; j <= ferm[i].Length - 1; j++)
                    if (ferm[i][j] == ',')
                    {
                        level[q, w] = Int32.Parse(str);
                        str = "";
                        if (w < m - 1)
                            w++;
                        else
                        {
                            w = 0;
                            if (q < n - 1)
                                q++;
                        }
                    }
                    else
                        str += ferm[i][j];
            }

            for (int i = n - 1; i >= 0; i--)
            {
                for (int j = m - 1; j >= 0; j--)
                {
                    int ch = level[i, j];
                    switch (ch)
                    {
                        case -1:
                            continue;

                        case 1:
                            Game.Components.Add(new Wall(Game, ref texture, new Rectangle(32, 0, 32, 32), new Vector2(j * 32, i * 32)));// стена
                            break;

                        case 2:
                            Game.Components.Add(new Ladder(Game, ref texture, new Rectangle(64, 0, 32, 32), new Vector2(j * 32, i * 32))); // лестница
                            break;

                        case 3:
                            Game.Components.Add(new Wall(Game, ref texture, new Rectangle(96, 0, 32, 32), new Vector2(j * 32, i * 32))); // бардюр
                            break;

                        case 4:
                            Game.Components.Add(new Wall(Game, ref texture, new Rectangle(128, 0, 32, 32), new Vector2(j * 32, i * 32))); // металлическая балка
                            break;

                        case 5:
                            Game.Components.Add(new Rubbish(Game, ref texture, new Rectangle(160, 0, 32, 32), new Vector2(j * 32, i * 32))); // мусор
                            break;

                        case 10:
                            Game.Components.Add(new InvicibleWall(Game, ref texture, new Rectangle(320, 0, 32, 32), new Vector2(j * 32, i * 32))); // нижняя часть тротуара
                            break;

                        case 24:
                            Game.Components.Add(new Wall(Game, ref texture, new Rectangle(768, 0, 32, 32), new Vector2(j * 32, i * 32))); // земля с травой
                            break;

                        case 25:
                            Game.Components.Add(new Grass(Game, ref texture, new Rectangle(800, 0, 32, 32), new Vector2(j * 32, i * 32))); // трава
                            break;

                        case 27:
                            Game.Components.Add(new InvicibleWall(Game, ref texture, new Rectangle(864, 0, 32, 32), new Vector2(j * 32, i * 32))); // задняя стена
                            break;

                        case 45:
                            Game.Components.Add(new Box(Game, ref texture, new Rectangle(448, 32, 64, 64), new Vector2(j * 32, i * 32))); // коробка
                            break;

                        case 48:
                            Game.Components.Add(new Lamp(Game, ref texture, new Rectangle(544, 32, 32, 128), new Vector2(j * 32, i * 32))); // фонарь 
                            break;

                        case 93:
                            Game.Components.Add(new Enemy(Game, ref texture, new Rectangle(0, 96, 32, 64), new Vector2(j * 32, i * 32))); // враг
                            break;

                        case 155:
                            Game.Components.Add(new NLO(Game, ref texture, new Rectangle(0, 160, 178, 80), new Vector2(j * 32, i * 32))); // летающая тарелка 
                            break;

                        case 248:
                            Game.Components.Add(new Alien(Game, ref texture, new Rectangle(0, 258, 32, 62), new Vector2(j * 32, i * 32))); // пришелец
                            break;

                        case 806:
                            Game.Components.Add(new SpecialForEnemy(Game, ref texture, new Rectangle(0, 832, 32, 32), new Vector2(j * 32, i * 32))); // стена для ботов
                            break;
                    }
                }
            }
        }
    }
}
