﻿using System;
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
        public Vector2[,] Cellules { get; set; }
        public Vector2 TailleTiles { get; set; }
        public Vector2 Taille { get; set; }
        public List<Unite> unites { get; set; }
        public List<Batiment> batiments { get; set; }
        public Noeud[,] unitesStatic { get; set; }
        public World world { get; set; }
        public GameTime gametime { get; set; }

        public int compteur = 0;
        public int pFParThread = 5; // 10 unites par thread calculent leurs chemins
        private byte[] tab;

        public Map(Texture2D sprite, Vector2 taille, Vector2 tailleTiles)
        {
            Sprite = sprite;
            Cellules = new Vector2[(int)taille.X, (int)taille.Y];
            TailleTiles = tailleTiles;
            Taille = taille;
            world = new World(Vector2.Zero);
            unites = new List<Unite> { };
            batiments = new List<Batiment> { };
            unitesStatic = new Noeud[(int)taille.X, (int)taille.Y];
            tab = new byte[] { 3, 4,6,7,8,9,10};

            foreach (Batiment b in batiments)
                unitesStatic[(int)b.PositionTile.X, (int)b.PositionTile.Y] = new Noeud(b.PositionTile, false, 1);
        }

        public void Update(List<Unite> unites, List<Batiment> batiments, GameTime GT)
        {
            this.unites = unites;
            this.batiments = batiments;
            Outil.RemoveDeadBodies(unites);
            gametime = GT;

            compteur += pFParThread; // Update du compteur pour le pF
            int temp = PLusGrosId(unites);
            compteur %= (temp != 0) ? temp : 1; // pour eviter d'avoir des unités inactives

            KeyboardState k = Keyboard.GetState();
            // Debug les unites qui attaquent des unites mortes
            foreach (Unite u in unites)
            {
                // affiche la barre des sorts des unités attawquant un champion
                if (u.uniteAttacked != null && u.uniteAttacked.isAChamp)
                    u.Drawlife = true;
                if (u.uniteAttacked != null && !unites.Contains((Unite)u.uniteAttacked))
                    u.uniteAttacked = null;
            }
        }

        static int PLusGrosId(List<Unite> liste)
        {
            List<int> newList = new List<int> { };

            var requete = from u in liste
                          orderby u.id
                          select new { u.id };

            foreach (var n in requete)
                newList.Add(n.id);

            return (newList.Count == 0) ? 0 : newList[newList.Count - 1];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Cellules.GetLength(0) + 2; i++) //On parcourt les lignes du tableau
                for (int j = 0; j < Cellules.GetLength(1) + 1; j++) //On parcourt les colonnes du tableau
                {
                    // Pour completer le bord droit et bas
                    if (i >= Cellules.GetLength(0) || j == Cellules.GetLength(1) )
                    {
                        Random rand = new Random(i * j);
                        int x = 32 * tab[rand.Next(1, tab.Length - 1)];
                        int y = 32 * 19; // a choisir en fonction de ce qu'on veut
                        spriteBatch.Draw(Sprite, new Vector2(i * (TailleTiles.X - 1), j * (TailleTiles.Y - 1)), new Rectangle(x + (x / 32), y + (y / 32), 32, 32), Color.White);
                    }
                    else
                    {
                        // Si y'a une texture prédéfini
                        if (Cellules[i, j] != Vector2.Zero)
                        {
                            int x = 32 * (int)Cellules[i, j].X;
                            int y = 32 * (int)Cellules[i, j].Y;
                            spriteBatch.Draw(Sprite, new Vector2(i * (TailleTiles.X - 1), j * (TailleTiles.Y - 1)), new Rectangle(x + (x / 32), y + (y / 32), 32, 32), Color.White);
                        }
                        // Sinon texture par defaut
                        else
                        {
                            Random rand = new Random(i * j);
                            int x = 32 * tab[rand.Next(1, tab.Length - 1)];
                            int y = 32 * 19; // a choisir en fonction de ce qu'on veut
                            spriteBatch.Draw(Sprite, new Vector2(i * (TailleTiles.X - 1), j * (TailleTiles.Y - 1)), new Rectangle(x + (x / 32), y + (y / 32), 32, 32), Color.White);
                        }
                    }
                }
        }
    }
}
