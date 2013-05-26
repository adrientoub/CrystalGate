using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics;

namespace CrystalGate
{
    class Outil
    {

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
            return (float)Math.Sqrt(Math.Pow(32 * (point1.X - point2.X), 2) + Math.Pow(32 * (point1.Y - point2.Y), 2));
        }

        public static void RemoveDeadBodies(List<Unite> unites)
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

        public static string Normalize(int maxParLigne, string description)
        {
            string rendu = "";

            int i = 0;
            string[] descriptionWords = description.Split(new char[] { ' ' });

            for (int j = 0; j < descriptionWords.Length; j++)
            {
                if (descriptionWords[j].Length + i <= maxParLigne)
                {
                    rendu += descriptionWords[j] + " ";
                    i += descriptionWords[j].Length + 1;
                }
                else if (descriptionWords[j].Length <= maxParLigne)
                {
                    rendu += "\n" + descriptionWords[j] + " ";
                    i = descriptionWords[j].Length + 1;
                }
                else
                {
                    rendu += "\n";
                    i = 0;
                    foreach (char c in descriptionWords[j])
                    {
                        if (i < maxParLigne)
                        {
                            rendu += c;
                            i++;
                        }
                        else
                        {
                            rendu += "\n" + c.ToString();
                            i = 0;
                        }
                    }
                }
            }
            return rendu;
        }

        enum Direction
        {
            Haut,
            Bas,
            Gauche,
            Droite,
            HautDroite,
            BasDroite,
            HautGauche,
            BasGauche
        }
    }
}
