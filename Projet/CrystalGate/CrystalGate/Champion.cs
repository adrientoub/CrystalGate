using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Champion : Unite
    {

        public Champion(Texture2D Sprite, Vector2 Position, Map map, SpriteBatch spriteBatch, PackTexture packTexture)
            : base(Sprite, Position, map, spriteBatch, packTexture)
        {

        }

        public override void Update(List<Objet> unitsOnMap, List<Effet> effets)
        {
            Animer();
            Clavier();
            Deplacer();
        }

        public void Clavier()
        {
           /* KeyboardState k = Keyboard.GetState();
            GamePadState g = GamePad.GetState(PlayerIndex.One);

            if (k.IsKeyDown(Keys.Z) && k.IsKeyDown(Keys.D) && ObjectifListe.Count == 0)
            {
                List<Noeud> chemin = PathFinding.TrouverChemin(new Vector2((int)Position.X, (int)Position.Y), Position + new Vector2(1, -1), Map.Taille, Map.unites, true);
                if (chemin != null)
                    ObjectifListe = chemin;
            }

            else if (k.IsKeyDown(Keys.Z) && k.IsKeyDown(Keys.Q) && ObjectifListe.Count == 0)
            {
                List<Noeud> chemin = PathFinding.TrouverChemin(new Vector2((int)Position.X, (int)Position.Y), Position + new Vector2(-1, -1), Map.Taille, Map.unites, true);
                if (chemin != null)
                    ObjectifListe = chemin;
            }

            else if (k.IsKeyDown(Keys.S) && k.IsKeyDown(Keys.D) && ObjectifListe.Count == 0)
            {
                List<Noeud> chemin = PathFinding.TrouverChemin(new Vector2((int)Position.X, (int)Position.Y), Position + new Vector2(1, 1), Map.Taille, Map.unites, true);
                if (chemin != null)
                    ObjectifListe = chemin;
            }

            else if (k.IsKeyDown(Keys.S) && k.IsKeyDown(Keys.Q) && ObjectifListe.Count == 0)
            {
                List<Noeud> chemin = PathFinding.TrouverChemin(new Vector2((int)Position.X, (int)Position.Y), Position + new Vector2(-1, 1), Map.Taille, Map.unites, true);
                if (chemin != null)
                    ObjectifListe = chemin;
            }

            else if (k.IsKeyDown(Keys.Q) && ObjectifListe.Count == 0)
            {
                List<Noeud> chemin = PathFinding.TrouverChemin(new Vector2((int)Position.X, (int)Position.Y), Position + new Vector2(-1, 0), Map.Taille, Map.unites, true);
                if (chemin != null)
                    ObjectifListe = chemin;

            }

            else if (k.IsKeyDown(Keys.D) && ObjectifListe.Count == 0)
            {
                List<Noeud> chemin = PathFinding.TrouverChemin(new Vector2((int)Position.X, (int)Position.Y), Position + new Vector2(1, 0), Map.Taille, Map.unites, true);
                if (chemin != null)
                    ObjectifListe = chemin;

            }

            else if (k.IsKeyDown(Keys.Z) && ObjectifListe.Count == 0)
            {
                List<Noeud> chemin = PathFinding.TrouverChemin(new Vector2((int)Position.X, (int)Position.Y), Position + new Vector2(0, -1), Map.Taille, Map.unites, true);
                if (chemin != null)
                    ObjectifListe = chemin;
            }

            else if (k.IsKeyDown(Keys.S) && ObjectifListe.Count == 0)
            {
                List<Noeud> chemin = PathFinding.TrouverChemin(new Vector2((int)Position.X, (int)Position.Y), Position + new Vector2(0, 1), Map.Taille, Map.unites, true);
                if (chemin != null)
                    ObjectifListe = chemin;
            }

            else if (k.IsKeyDown(Keys.A))
                Attaquer();*/

        }
    }
}

    

