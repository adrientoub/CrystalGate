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
    public static class PackAnimation
    {
        /* Le packAnimation est un pack qui regroupe les differentes positions de la sprite
           à Draw pour afficher l'animation correspondante */
        public static bool isArcher;

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

        // METTRE 7 POUR ARCHER
        public static List<Vector2> AttaquerHaut()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 5; j <= (isArcher ? 6 : 8) ; j++)
                liste.Add(new Vector2(0, j));

            return liste;
        }

        public static List<Vector2> AttaquerBas()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 5; j <= (isArcher ? 6 : 8); j++)
                liste.Add(new Vector2(4, j));

            return liste;
        }

        public static List<Vector2> AttaquerDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 5; j <= (isArcher ? 6 : 8); j++)
                liste.Add(new Vector2(2, j));

            return liste;
        }

        // MORT

        public static List<Vector2> Mort(Unite unite)
        {
            List<Vector2> liste = new List<Vector2> { };
            if(unite is Cavalier)
                liste.Add(new Vector2(0, 10));
            else if (unite is Archer)
                liste.Add(new Vector2(4, 7));
            else if (unite is Troll)
                liste.Add(new Vector2(4, 9));
            else if (unite is Grunt)
                liste.Add(new Vector2(0, 10));
            else if (unite is Demon)
                liste.Add(new Vector2(0, 10));
            else if (unite is Ogre)
                liste.Add(new Vector2(0, 10));
            else if (unite is Guerrier)
                liste.Add(new Vector2(0, 10));
            else
                throw new Exception("T'as pas modif la classe packanimation!");

            return liste;
        }

        public static List<Vector2> Explosion()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 5; j++)
                for (int i = 0; i < 5; i++)
                    liste.Add(new Vector2(i, j));

            return liste;
        }

        public static List<Vector2> Soin()
        {
            List<Vector2> liste = new List<Vector2> { };
                for (int i = 0; i < 6; i++)
                    liste.Add(new Vector2(i, 0));

            return liste;
        }
    }
}
