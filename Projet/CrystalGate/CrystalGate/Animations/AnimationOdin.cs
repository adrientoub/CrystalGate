using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate.Animations
{
    class AnimationOdin : PackAnimation
    {
        public override List<Vector2> StandHaut()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        public override List<Vector2> StandBas()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        public override List<Vector2> StandGauche()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        public override List<Vector2> StandDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }


        // MOUVEMENTS

        public override List<Vector2> Haut()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        public override List<Vector2> Bas()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        public override List<Vector2> Droite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        public override List<Vector2> HautDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        public override List<Vector2> BasDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        // ATTAQUER

        public override List<Vector2> AttaquerHaut()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        public override List<Vector2> AttaquerBas()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }

        public override List<Vector2> AttaquerDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 0; j < 3; j++)
                liste.Add(new Vector2(j, 0));

            return liste;
        }
    }
}
