using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class PathFinding
    {
        public static Noeud[,] Initialiser(int taille, Vector2 arrivee, List<Objet> unites)
        {
            Noeud[,] map = new Noeud[taille, taille];
            List<Noeud> unitToNoeud = new List<Noeud> { };
                foreach (Unite o in unites)
                    if (o != null)
                    {
                        Vector2 lol = new Vector2((int)(ConvertUnits.ToDisplayUnits(o.body.Position.X) / 32), (int)(ConvertUnits.ToDisplayUnits(o.body.Position.Y) / 32));
                        unitToNoeud.Add(new Noeud(lol, false, 1));
                    }

            for (int j = 0; j < taille; j++)
                for (int i = 0; i < taille; i++)
                {
                    if (NoeudInList(new Noeud(new Vector2(i,j), false, 0), unitToNoeud))
                        map[i, j] = new Noeud(new Vector2(i, j), false, GetHeuristic(arrivee, new Vector2(i, j)));
                    else
                        map[i, j] = new Noeud(new Vector2(i, j), true, GetHeuristic(arrivee, new Vector2(i, j)));
                }
        
            return map;
        }
        // Aide à deboguer
        public static string Draw(Noeud[,] map)
        {
            string mapString = "";
            for (int j = 0; j < map.GetLength(1); j++)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                    if (map[i,j].IsWalkable)
                        mapString += " ";
                    else
                        mapString += "X";

                mapString += "\n";
            }
            mapString += "\n";
            return mapString;
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

        static List<Noeud> Voisins(Noeud current, Noeud[,] map, bool enableDiagonales)
        {
            Noeud noeud = null;
            List<Noeud> voisins = new List<Noeud> { };
            // TOP
            if (current.Position.Y - 1 >= 0 && map[(int)current.Position.X, (int)current.Position.Y - 1].IsWalkable)
            {
                noeud = map[(int)current.Position.X, (int)current.Position.Y - 1];
                voisins.Add(noeud);
            }
            // RIGHT
            if (current.Position.X + 1 < map.GetLength(0) && map[(int)current.Position.X + 1, (int)current.Position.Y].IsWalkable)
            {
                noeud = map[(int)current.Position.X + 1, (int)current.Position.Y];
                voisins.Add(noeud);
            }
            // BOT
            if (current.Position.Y + 1 < map.GetLength(1) && map[(int)current.Position.X, (int)current.Position.Y + 1].IsWalkable)
            {
                noeud = map[(int)current.Position.X, (int)current.Position.Y + 1];
                voisins.Add(noeud);
            }
            // LEFT
            if (current.Position.X - 1 >= 0 && map[(int)current.Position.X - 1, (int)current.Position.Y].IsWalkable)
            {
                noeud = map[(int)current.Position.X - 1, (int)current.Position.Y];
                voisins.Add(noeud);
            }
                // TOP LEFT
                if (current.Position.Y - 1 >= 0 && current.Position.X - 1 >= 0 && map[(int)current.Position.X - 1, (int)current.Position.Y - 1].IsWalkable)
                {
                    noeud = map[(int)current.Position.X - 1, (int)current.Position.Y - 1];
                    voisins.Add(noeud);
                }
                // TOP RIGHT
                if (current.Position.Y - 1 >= 0 && current.Position.X + 1 < map.GetLength(0) && map[(int)current.Position.X + 1, (int)current.Position.Y - 1].IsWalkable)
                {
                    noeud = map[(int)current.Position.X + 1, (int)current.Position.Y - 1];
                    voisins.Add(noeud);
                }
                // BOT LEFT
                if (current.Position.X - 1 >= 0 && current.Position.Y + 1 < map.GetLength(1) && map[(int)current.Position.X - 1, (int)current.Position.Y + 1].IsWalkable)
                {
                    noeud = map[(int)current.Position.X - 1, (int)current.Position.Y + 1];
                    voisins.Add(noeud);
                }
                // BOT RIGHT
                if (current.Position.X + 1 < map.GetLength(0) && current.Position.Y + 1 < map.GetLength(1) && map[(int)current.Position.X + 1, (int)current.Position.Y + 1].IsWalkable)
                {
                    noeud = map[(int)current.Position.X + 1, (int)current.Position.Y + 1];
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

        public static List<Noeud> TrouverChemin(Vector2 depart, Vector2 arrivee, int taille, List<Objet> unites, bool champion)
        {

            Noeud[,] map = Initialiser(taille, arrivee, unites); // INITIALISATION DU POIDS DES NOEUDS ET DES OBSTACLES
            List<Noeud> closedList = new List<Noeud> { };

            List<Noeud> openList = new List<Noeud> { map[(int)depart.X,(int)depart.Y] };

            if (arrivee.X >= 0 && arrivee.Y >= 0 && arrivee.X < taille && arrivee.Y < taille && map[(int)arrivee.X, (int)arrivee.Y].IsWalkable) // SI ON NE SORT PAS DE LA MAP ET QUE L'ON PEUT MARCHER SUR LE POINT D'ARRIVEE
            {
                Noeud noeudDepart = map[(int)depart.X, (int)depart.Y];
                Noeud noeudArrivee = map[(int)arrivee.X, (int)arrivee.Y];

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

                    List<Noeud> voisins = Voisins(current, map, champion);
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
