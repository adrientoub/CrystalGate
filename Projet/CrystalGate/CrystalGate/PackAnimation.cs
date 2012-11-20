using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;


namespace CrystalGate
{
    public class PackAnimation
    {
        // STAND

        public static List<Vector2> StandHaut()
        {
            List<Vector2> liste = new List<Vector2> { };
                liste.Add(new Vector2(0, 0));

            return liste;
        }

        public static List<Vector2> StandBas()
        {
            List<Vector2> liste = new List<Vector2> { };
            liste.Add(new Vector2(4, 0));

            return liste;
        }

        public static List<Vector2> StandGauche()
        {
            List<Vector2> liste = new List<Vector2> { };
            liste.Add(new Vector2(2, 0));

            return liste;
        }

        public static List<Vector2> StandDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            liste.Add(new Vector2(2, 0));

            return liste;
        }


        // MOUVEMENTS

        public static List<Vector2> Haut()
        {
            List<Vector2> liste = new List<Vector2>{};
            for (int j = 0; j <= 4; j++)
                liste.Add(new Vector2(0, j));

            return liste;
        }

        public static List<Vector2> Bas()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j <= 4; j++)
                liste.Add(new Vector2(4, j));

            return liste;
        }

        public static List<Vector2> Droite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j <= 4; j++)
                liste.Add(new Vector2(2, j));

            return liste;
        }

        public static List<Vector2> HautDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j <= 4; j++)
                liste.Add(new Vector2(1, j));

            return liste;
        }

        public static List<Vector2> BasDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j <= 4; j++)
                liste.Add(new Vector2(3, j));

            return liste;
        }

        // ATTAQUER

        public static List<Vector2> AttaquerHaut()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 5; j <= 8; j++)
                liste.Add(new Vector2(0, j));

            return liste;
        }

        public static List<Vector2> AttaquerBas()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 5; j <= 8; j++)
                liste.Add(new Vector2(4, j));

            return liste;
        }

        public static List<Vector2> AttaquerDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 5; j <= 8; j++)
                liste.Add(new Vector2(2, j));

            return liste;
        }

        // MORT

        public static List<Vector2> Mort()
        {
            List<Vector2> liste = new List<Vector2> { };
                liste.Add(new Vector2(0, 10));

            return liste;
        }
    }
}
