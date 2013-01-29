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

namespace CrystalGate
{
    public class Map
    {
        Texture2D Sprite { get; set; }
        public Vector2[,] Cellules { get; set; }
        public Vector2 TailleTiles { get; set; }
        public Vector2 Taille { get; set; }
        public List<Unite> unites { get; set; }
        public Noeud[,] unitesStatic { get; set; }
        public World world { get; set; }
        Body boundary; // Les limites du monde physique
        public GameTime gametime { get; set; }

        public int compteur = 0;
        public int pFParThread = 5; // 10 unites par thread calculent leurs chemins

        public Map(Texture2D sprite, Vector2 taille, Vector2 tailleTiles)
        {
            Sprite = sprite;
            Cellules = new Vector2[(int)taille.X, (int)taille.Y];
            TailleTiles = tailleTiles;
            Taille = taille;
            world = new World(Vector2.Zero);
            unites = new List<Unite> { };
            unitesStatic = new Noeud[(int)taille.X, (int)taille.Y];

            // Creation de la physique de la carte
            var bounds = GetBounds();
            boundary = BodyFactory.CreateLoopShape(world, bounds);
            boundary.CollisionCategories = Category.All;
            boundary.CollidesWith = Category.All;

        }

        public void Update(List<Unite> unites, GameTime GT)
        {
            this.unites = unites;
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
            for (int i = 0; i < Cellules.GetLength(0); i++) //On parcourt les lignes du tableau
                for (int j = 0; j < Cellules.GetLength(1); j++) //On parcourt les colonnes du tableau
                {
                        int x = (int)TailleTiles.X * (int)Cellules[i, j].X;
                        int y = (int)TailleTiles.Y * (int)Cellules[i, j].Y;
                        spriteBatch.Draw(Sprite, new Vector2(i * (TailleTiles.X - 1), j * (TailleTiles.Y - 1)), new Rectangle(x + (x / 32), y + (y / 32), 32, 32), Color.White);
                }
        }

        // Utilisé pour creer le monde physique
        private Vertices GetBounds()
        {
            float width = ConvertUnits.ToSimUnits(Taille.X * TailleTiles.X);
            float height = ConvertUnits.ToSimUnits(Taille.Y * TailleTiles.Y);

            Vertices bounds = new Vertices(4);
            bounds.Add(new Vector2(0, 0));
            bounds.Add(new Vector2(width, 0));
            bounds.Add(new Vector2(width, height));
            bounds.Add(new Vector2(0, height));

            return bounds;
        }
    }


}
