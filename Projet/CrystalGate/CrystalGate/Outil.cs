using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    class Outil
    {
        public static bool Collision(Objet box1, Objet box2)
        {
            if ((box2.body.Position.X >= box1.body.Position.X + ConvertUnits.ToSimUnits(32))      // trop à droite
             || (box2.body.Position.X + ConvertUnits.ToSimUnits(32) <= box1.body.Position.X) // trop à gauche
             || (box2.body.Position.Y >= box1.body.Position.Y + ConvertUnits.ToSimUnits(32)) // trop en bas
             || (box2.body.Position.Y + ConvertUnits.ToSimUnits(32) <= box1.body.Position.Y))  // trop en haut
                return false;
            else
                return true;
        }

        public static float AngleUnites(Objet unite1, Objet unite2)
        {
            return (float)Math.Atan2(Math.Abs(unite1.body.Position.Y - unite2.body.Position.Y), Math.Abs(unite1.body.Position.X - unite2.body.Position.X));
        }

        public static float DistanceUnites(Objet unite1, Objet unite2)
        {
            return (float)Math.Sqrt(Math.Pow(ConvertUnits.ToDisplayUnits(unite1.body.Position - unite2.body.Position).X, 2) + Math.Pow(ConvertUnits.ToDisplayUnits(unite1.body.Position - unite2.body.Position).Y, 2));
        }

        public static void RemoveDeadBodies(List<Objet> unites)
        {
            for (int i = 0; i < unites.Count; i++)
                if (unites[i].Mort)
                    unites.RemoveAt(i);
        }

        /*public static void CheckCollision(List<Objet> unites)
        {
            Unite same = null;

            foreach(Unite u in unites)
                foreach (Unite u2 in unites)
                    if (u != u2 && same != u)
                        if (Collision(u, u2))
                        {
                            same = u2;
                            u.collideWith = u2;
                            u2.collideWith = u;

                            if (u.ObjectifListe.Count > 0)
                            {
                                Vector2 position = new Vector2((int)(ConvertUnits.ToDisplayUnits(u.body.Position.X) / 32), (int)(ConvertUnits.ToDisplayUnits(u.body.Position.Y) / 32));
                                List<Noeud> lol = PathFinding.TrouverChemin(position, u.ObjectifListe[u.ObjectifListe.Count - 1].Position, u.Map.Taille, new List<Objet> { u2 }, false);
                                if (lol != null)
                                    u.ObjectifListe = lol;
                            }

                            
                        }
                    else
                        continue;
        }

        public static void DebugCollision(List<Objet> unites)
        {
            List<Unite> unitesConvert = ListObjetToUnite(unites);

            foreach (Unite u in unitesConvert)
                if (u.collideWith != null && !Collision(u, u.collideWith))
                    u.collideWith = null;
        }*/
    }
}
