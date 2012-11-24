using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace CrystalGate
{
    public class Map
    {
        Texture2D Sprite { get; set; }
        int[,] Cellules { get; set; }
        public Vector2 TailleTiles { get; set; }
        public int Taille { get; set; }
        public List<Objet> unites { get; set; }
        public World world { get; set; }

        public Map(Texture2D sprite, int taille, Vector2 tailleTiles)
        {
            Sprite = sprite;
            Cellules = new int[taille, taille];
            TailleTiles = tailleTiles;
            Taille = taille;
            world = new World(Vector2.Zero);
        }

        public void Update(List<Objet> unites)
        {
            this.unites = unites;
            Outil.RemoveDeadBodies(unites);
        }

        public void ClearEffects()
        {

        }

        /*public void DrawIso(SpriteBatch spriteBatch)
        {
            int lengthx = 168;
            int lengthy = 89;
                
            Vector2 _posDepart = new Vector2(100, 100);
            for (int i = 0; i < Cellules.GetLength(0); i++) //On parcourt les lignes du tableau
                for (int j = 0; j < Cellules.GetLength(1); j++) //On parcourt les colonnes du tableau
                {
                    if (Cellules[i, j] != -1)
                    {
                        Vector2 pos = new Vector2();
                        if (i != 0)
                        {
                            if (i % 2 != 0 || i == 1) //Si le numéro de ligne est impair
                            {
                                pos.X = ((j * lengthx + lengthx / 2) + (((i - 1) * lengthx) / 2)) - j * lengthx / 2 + _posDepart.X;
                                pos.Y = (((i * lengthy) / 2)) - j * lengthy / 2 + _posDepart.Y;
                            }
                            else //Sinon s'il est pair
                            {
                                pos.X = (j * lengthx + ((i * lengthx) / 2)) - j * lengthx / 2 + _posDepart.X;
                                pos.Y = (((i * lengthy) / 2)) - j * lengthy / 2 + _posDepart.Y;
                            }
                        }
                        else //Si il est égal à zéro (même code que s'il est pair)
                        {
                            pos.X = (j * lengthx) - j * lengthx / 2 + _posDepart.X;
                            pos.Y = (((i * lengthy) / 2)) - j * lengthy / 2 + _posDepart.Y;
                        }
                        spriteBatch.Draw(Sprite, pos, Color.White);
                    }
                }

        }*/

        public void Draw(SpriteBatch spriteBatch)
        {
            int lengthx = 32;
            int lengthy = 32;

            Vector2 _posDepart = new Vector2(100, 100);
            for (int i = 0; i < Cellules.GetLength(0); i++) //On parcourt les lignes du tableau
                for (int j = 0; j < Cellules.GetLength(1); j++) //On parcourt les colonnes du tableau
                        spriteBatch.Draw(Sprite, new Vector2(i * lengthx, j * lengthy), Color.White);

        }
    }
}
