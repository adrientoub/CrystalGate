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
        public MouseState mouse { get; set; }
        public Unite Selection { get; set; }
        public Map map { get; set; }
        public List<Objet> unites { get; set; }

        public Joueur(Map map)
        {
            this.map = map;
            mouse = Mouse.GetState();
        }

        public void Update(List<Objet> unites)
        {
            mouse = Mouse.GetState();

            //if (mouse.LeftButton == ButtonState.Pressed)
                UpdateSelection(unites);

            if (mouse.RightButton == ButtonState.Pressed)
                DonnerOrdreDeplacer(Selection, map);
            this.unites = unites;
        }

        private void UpdateSelection(List<Objet> unites)
        {
            /*foreach (Unite u in unites)
                if ((int)(mouse.X / map.TailleTiles.X) == (int)u.PositionTile.X && (int)(mouse.Y / map.TailleTiles.Y) == (int)u.PositionTile.Y)
                    Selection = u;*/
            Selection = (Unite)unites[0];
            ((Unite)unites[0]).Drawlife = true;
        }

        public void DonnerOrdreDeplacer(Unite unite, Map map)
        {

            if (Selection != null)
            {
                Vector2 lol = new Vector2((int)(ConvertUnits.ToDisplayUnits(unite.body.Position.X) / 32), (int)(ConvertUnits.ToDisplayUnits(unite.body.Position.Y) / 32));
                Vector2 ObjectifPoint = new Vector2(mouse.X, mouse.Y) / map.TailleTiles;
                ObjectifPoint = new Vector2((int)ObjectifPoint.X, (int)ObjectifPoint.Y);
                List<Noeud> chemin = PathFinding.TrouverChemin(lol, ObjectifPoint, unite.Map.Taille, new List<Objet> { }, false);
                if (chemin != null)
                    unite.ObjectifListe = chemin;
            }
        }
    }
}
