using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Map
    {
        Texture2D Sprite { get; set; }
        int[,] Cellules { get; set; }
        public Vector2 TailleTiles { get; set; }
        public Vector2 Taille { get; set; }
        public List<Objet> unites { get; set; }
        public World world { get; set; }
        public GameTime gametime { get; set; }

        public int compteur = 0;
        public int pFParThread = 10; // 10 unites par thread calculent leurs chemins
        private byte[] tab;

        public Map(Texture2D sprite, Vector2 taille, Vector2 tailleTiles)
        {
            Sprite = sprite;
            Cellules = new int[(int)taille.X, (int)taille.Y];
            TailleTiles = tailleTiles;
            Taille = taille;
            world = new World(Vector2.Zero);
            unites = new List<Objet> { };
            tab = new byte[] { 3, 4,6,7,8,9,10};
        }

        public void Update(List<Objet> unites, GameTime GT)
        {
            this.unites = unites;
            Outil.RemoveDeadBodies(unites);
            gametime = GT;

            compteur += pFParThread; // Update du compteur pour le pF
            int temp = PLusGrosId(unites);
            compteur %= (temp != 0) ? temp : 1; // pour eviter d'avoir des unités inactives

            KeyboardState k = Keyboard.GetState();

            foreach (Unite u in unites)
                if (u.uniteAttacked != null && !unites.Contains((Unite)u.uniteAttacked))
                    u.uniteAttacked = null;
        }

        public void ClearEffects()
        {

        }

        static int PLusGrosId(List<Objet> liste)
        {
            List<int> newList = new List<int> { };

            var requete = from u in liste
                          orderby u.id
                          select new { u.id };

            foreach (var n in requete)
                newList.Add(n.id);

            return (newList.Count == 0) ? 0 : newList[newList.Count - 1];
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
            for (int i = 0; i < Cellules.GetLength(0); i++) //On parcourt les lignes du tableau
                for (int j = 0; j < Cellules.GetLength(1); j++) //On parcourt les colonnes du tableau
                {
                    Random rand = new Random( i * j);
                    int x = 32 * tab[rand.Next(1,tab.Length - 1)];
                    int y = 32 * 19; // a choisir en fonction de ce qu'on veut
                    spriteBatch.Draw(Sprite, new Vector2(i * TailleTiles.X, j * TailleTiles.Y), new Rectangle(x + (x / 32), y + (y / 32), 32, 32), Color.White);
                }
        }
    }
}
