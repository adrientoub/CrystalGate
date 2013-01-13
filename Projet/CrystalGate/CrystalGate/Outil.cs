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
            return (float)Math.Atan2(unite1.body.Position.Y - unite2.body.Position.Y, unite1.body.Position.X - unite2.body.Position.X);
        }

        public static float DistanceUnites(Objet unite1, Objet unite2)
        {
            return (float)Math.Sqrt(Math.Pow(ConvertUnits.ToDisplayUnits(unite1.body.Position - unite2.body.Position).X, 2) + Math.Pow(ConvertUnits.ToDisplayUnits(unite1.body.Position - unite2.body.Position).Y, 2));
        }
        public static float DistancePoints(Vector2 point1, Vector2 point2)
        {
            return (float)Math.Sqrt( Math.Pow(32 * (point1.X - point2.X), 2) + Math.Pow(32 * (point1.Y - point2.Y), 2));
        }

        public static void RemoveDeadBodies(List<Objet> unites)
        {
            for (int i = 0; i < unites.Count; i++)
                if (unites[i].Mort)
                    unites.RemoveAt(i);
        }

        public static List<Unite> ObjetToUnits(List<Objet> objets)
        {
            List<Unite> u = new List<Unite> { };
            foreach (Objet o in objets)
                u.Add((Unite)o);

            return u;
        }
    }
}
