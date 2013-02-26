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
        public List<Joueur> joueurs;
        public List<Unite> unites;
        public List<Effet> effets; // effets (cadavres) qui seront draw
        public List<Item> items;
        public List<Wave> waves;
        public Noeud[,] unitesStatic { get; set; }
        public World world { get; set; }
        Body boundary; // Les limites du monde physique
        public GameTime gametime { get; set; }

        public Map(Texture2D sprite, Vector2 taille, Vector2 tailleTiles)
        {
            Sprite = sprite;
            Cellules = new Vector2[(int)taille.X, (int)taille.Y];
            TailleTiles = tailleTiles;
            Taille = taille;
            world = new World(Vector2.Zero);
            joueurs = new List<Joueur> { };
            unites = new List<Unite> { };
            effets = new List<Effet> { };
            items = new List<Item> { };
            waves = new List<Wave> { };
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
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Cellules.GetLength(0); i++) //On parcourt les lignes du tableau
                for (int j = 0; j < Cellules.GetLength(1); j++) //On parcourt les colonnes du tableau
                {
                        int x = (int)TailleTiles.X * (int)Cellules[i, j].X;
                        int y = (int)TailleTiles.Y * (int)Cellules[i, j].Y;
                        spriteBatch.Draw(Sprite, new Vector2(i * (TailleTiles.X), j * (TailleTiles.Y)), new Rectangle(x + (x / 32), y + (y / 32), 32, 32), Color.White);
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
