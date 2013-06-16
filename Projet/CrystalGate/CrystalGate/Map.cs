using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using CrystalGate.SceneEngine2;

namespace CrystalGate
{
    public static class Map
    {
        // Geré par Initialize :
        public static Texture2D Sprite;
        public static Vector2[,] Cellules;
        public static Vector2 TailleTiles = new Vector2(32, 32);
        public static Vector2 Taille;
        public static Noeud[,] unitesStatic;
        public static World world;
        public static TypeDeTerrain typeDeTerrain;

        // Geré par LoadLevel
        public static List<Unite> unites;
        public static List<Effet> effets; // effets (cadavres) qui seront draw
        public static List<Item> items;
        public static List<Wave> waves;
        public static int waveNumber = 0;
        public static int nombreDeVaguesPop, nombreDeVagues;
        public static GameTime gametime;

        public static byte id = 1;

        public static void Initialize()
        {
            //ici
            // Creation de la physique de la carte
            /*var bounds = GetBounds();
            boundary = BodyFactory.CreateLoopShape(world, bounds);
            boundary.CollisionCategories = Category.All;
            boundary.CollidesWith = Category.All;*/

        }

        public static void Update(GameTime GT)
        {
            Outil.RemoveDeadBodies(unites);
            gametime = GT;
            // Debug les unites qui attaquent des unites mortes
            foreach (Unite u in unites)
            {
                // affiche la barre des sorts des unités attaquant un champion
                if (u.uniteAttacked != null && u.uniteAttacked.isAChamp)
                    u.Drawlife = true;
                if (u.uniteAttacked != null && !unites.Contains((Unite)u.uniteAttacked))
                    u.uniteAttacked = null;
            }
            for (int i = 0; i < items.Count; i++)
                if (items[i].InInventory)
                    items.RemoveAt(i);

            // Dit si on a gagné , que toutes les vagues sont finies
            bool OnaWin = true;
            foreach(Wave w in waves)
                if (w.unites.Count != 0)
                {
                    OnaWin = false;
                    break;
                }

            if (OnaWin && SceneHandler.level == "level3")
                PackMap.joueurs[0].Interface.Win = true;
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Cellules.GetLength(0); i++) //On parcourt les lignes du tableau
                for (int j = 0; j < Cellules.GetLength(1); j++) //On parcourt les colonnes du tableau
                {
                        int x = (int)TailleTiles.X * (int)Cellules[i, j].X;
                        int y = (int)TailleTiles.Y * (int)Cellules[i, j].Y;
                        spriteBatch.Draw(Sprite, new Vector2(i * (TailleTiles.X), j * (TailleTiles.Y)), new Rectangle(x + (x / 32), y + (y / 32), 32, 32), Color.White);
                }
        }

        public static byte GetId()
        {
            id++;
            return (byte)(id - 1);
        }
        // Utilisé pour creer le monde physique
        private static Vertices GetBounds()
        {
            float width = ConvertUnits.ToSimUnits(100 * TailleTiles.X);
            float height = ConvertUnits.ToSimUnits(100 * TailleTiles.Y);

            Vertices bounds = new Vertices(4);
            bounds.Add(new Vector2(0, 0));
            bounds.Add(new Vector2(width, 0));
            bounds.Add(new Vector2(width, height));
            bounds.Add(new Vector2(0, height));

            return bounds;
        }

        public enum TypeDeTerrain
        {
            Herbe,
            Desert,
            Hiver,
            Volcanique
        }
    }


}
