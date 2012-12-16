using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Joueur
    {
        public Unite champion { get; set; }

        MouseState mouse { get; set; }

        public Joueur(Unite champ)
        {
            champion = champ;
            // Graphique
            champion.Sprite = champ.packTexture.unites[1];
            champion.Tiles = new Vector2(380 / 5, 600 / 11);

            // Statistiques
            champion.Vie = champ.VieMax = 100;
            champion.Vitesse = 2.0f;
            champion.Portee = 2f; // 2 = Corps à corps
            champion.Dommages = 20;
            champion.Drawlife = true;
            mouse = Mouse.GetState();
        }

        public void Update(List<Objet> unites)
        {
            mouse = Mouse.GetState();
            // Pour se déplacer
            if (mouse.RightButton == ButtonState.Pressed)
                DonnerOrdreDeplacer(champion, champion.Map);

            //this.unites = unites;
            /*foreach (Unite u in unites)
                if ((int)(mouse.X / map.TailleTiles.X) == (int)u.PositionTile.X && (int)(mouse.Y / map.TailleTiles.Y) == (int)u.PositionTile.Y)
        Selection = u;*/
        }

        public void DonnerOrdreDeplacer(Unite unite, Map map)
        {
            Vector2 ObjectifPoint = new Vector2(mouse.X, mouse.Y) / map.TailleTiles;
            //ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);
                
            List<Noeud> chemin = PathFinding.TrouverChemin(champion.PositionTile, ObjectifPoint, unite.Map.Taille, new List<Objet> { }, false);
                if (chemin != null)
                    unite.ObjectifListe = chemin;
        }
    }
}
