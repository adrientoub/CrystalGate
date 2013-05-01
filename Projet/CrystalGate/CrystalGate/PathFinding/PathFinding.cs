using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class PathFinding
    {
        public static Noeud[,] Initialiser(Noeud[,] Map, Vector2 taille, Vector2 arrivee, List<Unite> unites)
        {
            // Prend en compte les unites
                foreach (Unite o in unites)
                    if (o != null)
                    {// debug osutenance 3
                        if (o.PositionTile.X < 0)
                            o.PositionTile = new Vector2(0, o.PositionTile.Y);
                        if (o.PositionTile.Y < 0)
                            o.PositionTile = new Vector2(o.PositionTile.X, 0);
                        if (Map[(int)o.PositionTile.X, (int)o.PositionTile.Y] == null)
                            Map[(int)o.PositionTile.X, (int)o.PositionTile.Y] = new Noeud(o.PositionTile, false, 1);
                    }
            
            // Remplis les cases qui sont "marchables"
                AForge.Parallel.For(0, (int)taille.Y, delegate(int j)
                        {
                            for (int i = 0; i < taille.X; i++)
                                if (Map[i, j] == null)
                                        Map[i, j] = new Noeud(new Vector2(i, j), true, GetHeuristic(arrivee, new Vector2(i, j)));
                        });
        
            return Map;
        }
        // Aide à deboguer
        public static string Draw(Noeud[,] Map)
        {
            string MapString = "";
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                for (int i = 0; i < Map.GetLength(0); i++)
                    if (Map[i,j].IsWalkable)
                        MapString += " ";
                    else
                        MapString += "X";

                MapString += "\n";
            }
            MapString += "\n";
            return MapString;
        }

        static int GetHeuristic(Vector2 depart, Vector2 arrivee)
        {
            return (int)(Math.Abs(arrivee.X - depart.X) + Math.Abs(arrivee.Y - (int)depart.Y));
        }

        static bool NoeudInList(Noeud noeud, List<Noeud> liste)
        {
            bool result = false;

            foreach (Noeud n in liste)
                if (n.Position == noeud.Position)
                    result = true;

            return result;
        }

        static List<Noeud> Voisins(Noeud current, Noeud[,] Map, bool enableDiagonales)
        {
            Noeud noeud = null;
            List<Noeud> voisins = new List<Noeud> { };
            // TOP
            if (current.Position.Y - 1 >= 0 && Map[(int)current.Position.X, (int)current.Position.Y - 1].IsWalkable)
            {
                noeud = Map[(int)current.Position.X, (int)current.Position.Y - 1];
                voisins.Add(noeud);
            }
            // RIGHT
            if (current.Position.X + 1 < Map.GetLength(0) && Map[(int)current.Position.X + 1, (int)current.Position.Y].IsWalkable)
            {
                noeud = Map[(int)current.Position.X + 1, (int)current.Position.Y];
                voisins.Add(noeud);
            }
            // BOT
            if (current.Position.Y + 1 < Map.GetLength(1) && Map[(int)current.Position.X, (int)current.Position.Y + 1].IsWalkable)
            {
                noeud = Map[(int)current.Position.X, (int)current.Position.Y + 1];
                voisins.Add(noeud);
            }
            // LEFT
            if (current.Position.X - 1 >= 0 && Map[(int)current.Position.X - 1, (int)current.Position.Y].IsWalkable)
            {
                noeud = Map[(int)current.Position.X - 1, (int)current.Position.Y];
                voisins.Add(noeud);
            }
                // TOP LEFT
            if (current.Position.Y - 1 >= 0 && current.Position.X - 1 >= 0 
                && Map[(int)current.Position.X - 1, (int)current.Position.Y - 1].IsWalkable 
                && Map[(int)current.Position.X - 1, (int)current.Position.Y].IsWalkable
                && Map[(int)current.Position.X, (int)current.Position.Y - 1].IsWalkable)
                {
                    noeud = Map[(int)current.Position.X - 1, (int)current.Position.Y - 1];
                    noeud.CoutF -= 1;
                    voisins.Add(noeud);
                }
                // TOP RIGHT
                if (current.Position.Y - 1 >= 0 && current.Position.X + 1 < Map.GetLength(0) 
                    && Map[(int)current.Position.X + 1, (int)current.Position.Y - 1].IsWalkable
                    && Map[(int)current.Position.X, (int)current.Position.Y - 1].IsWalkable
                    && Map[(int)current.Position.X + 1, (int)current.Position.Y].IsWalkable)
                {
                    noeud = Map[(int)current.Position.X + 1, (int)current.Position.Y - 1];
                    noeud.CoutF -= 1;
                    voisins.Add(noeud);
                }
                // BOT LEFT
                if (current.Position.X - 1 >= 0 && current.Position.Y + 1 < Map.GetLength(1) 
                    && Map[(int)current.Position.X - 1, (int)current.Position.Y + 1].IsWalkable
                    && Map[(int)current.Position.X - 1, (int)current.Position.Y].IsWalkable
                    && Map[(int)current.Position.X, (int)current.Position.Y + 1].IsWalkable)
                {
                    noeud = Map[(int)current.Position.X - 1, (int)current.Position.Y + 1];
                    noeud.CoutF -= 1;
                    voisins.Add(noeud);
                }
                // BOT RIGHT
                if (current.Position.X + 1 < Map.GetLength(0) && current.Position.Y + 1 < Map.GetLength(1) 
                    && Map[(int)current.Position.X + 1, (int)current.Position.Y + 1].IsWalkable
                    && Map[(int)current.Position.X, (int)current.Position.Y + 1].IsWalkable
                    && Map[(int)current.Position.X + 1, (int)current.Position.Y].IsWalkable)
                {
                    noeud = Map[(int)current.Position.X + 1, (int)current.Position.Y + 1];
                    noeud.CoutF -= 1;
                    voisins.Add(noeud);
                }

            return voisins;
        }

        static List<Noeud> ReconstructPath(List<Noeud> ResultList, Noeud arrivee)
        {
            List<Noeud> TrueList = new List<Noeud> { };
            arrivee.Position = arrivee.Position;
            TrueList.Add(arrivee);
            FonctionChapeau(ResultList, arrivee, TrueList);
            TrueList.Reverse();

            return TrueList;
        }

        static void FonctionChapeau(List<Noeud> ResultList, Noeud arrivee, List<Noeud> TrueList)
        {
            if (arrivee.Parent != null)
            {
                if (NoeudInList(arrivee.Parent, ResultList))
                {
                    TrueList.Add(new Noeud(arrivee.Parent.Position, true, 0));
                    FonctionChapeau(ResultList, arrivee.Parent, TrueList);
                }
            }
            else
                TrueList.Add(arrivee);
        }

        static List<Noeud> PlusPetitNoeud(List<Noeud> listenoeud)
         {
             List<Noeud> newList = new List<Noeud> { };

            var requete = from noeud in listenoeud
                          orderby noeud.CoutF
                          select new { noeud.Position, noeud.IsWalkable, noeud.CoutF , noeud.Parent};

            foreach (var n in requete)
                newList.Add(new Noeud(n.Position, n.IsWalkable, n.CoutF, n.Parent));

            return newList;
        }

        public static List<Noeud> TrouverChemin(Vector2 depart, Vector2 arrivee, Vector2 taille, List<Unite> unites, Noeud[,] batiments, bool champion)
        {
            Noeud[,] Map = (Noeud[,])batiments.Clone(); // INITIALISATION DU POIDS DES NOEUDS ET DES OBSTACLES
            Initialiser(Map, taille, arrivee, unites);
            List<Noeud> closedList = new List<Noeud> { };
            // DEBUG SOUTENANCE PAS LE TEMPS
            if(depart.X < 0)
                depart = new Vector2(0, depart.Y);
            if (depart.Y < 0)
                depart = new Vector2(depart.X, 0);
            List<Noeud> openList = new List<Noeud> { Map[(int)depart.X,(int)depart.Y] };

            if (arrivee.X >= 0 && arrivee.Y >= 0 && arrivee.X < taille.X && arrivee.Y < taille.Y && Map[(int)arrivee.X, (int)arrivee.Y].IsWalkable) // SI ON NE SORT PAS DE LA Map ET QUE L'ON PEUT MARCHER SUR LE POINT D'ARRIVEE
            {
                Noeud noeudDepart = Map[(int)depart.X, (int)depart.Y];
                Noeud noeudArrivee = Map[(int)arrivee.X, (int)arrivee.Y];

                noeudDepart.CoutG = 0;
                noeudDepart.CoutF = noeudDepart.CoutG + GetHeuristic(depart, arrivee);

                while (openList.Count() != 0)
                {
                    openList = PlusPetitNoeud(openList);
                    Noeud current = openList[0];
                    if (current.Position == noeudArrivee.Position)
                    {
                        if(closedList.Count > 0)
                            closedList.RemoveAt(0);
                        closedList.Add(current);
                        return ReconstructPath(closedList, current);
                    }

                    openList.RemoveAt(0);
                    closedList.Add(current);

                    List<Noeud> voisins = Voisins(current, Map, champion);
                    foreach (Noeud v in voisins)
                        if (NoeudInList(v, closedList))
                            continue;
                        else
                        {
                            int tentative_g_score = current.CoutG + 1;
                            if (!NoeudInList(v, closedList) || tentative_g_score <= v.CoutG)
                            {
                                v.Parent = current;
                                v.CoutG = tentative_g_score;
                                v.CoutF = v.CoutG + GetHeuristic(v.Position, arrivee);
                                if (!NoeudInList(v, openList))
                                    openList.Add(v);
                            }
                        }
                }
            }
                return null;
        }
    }
}
