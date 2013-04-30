using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate.Animations
{
    class AnimationCritters : PackAnimation
    {
        Vector2 TailleTile = new Vector2(50, 50);
        // MOUVEMENTS

        public override List<Vector2> Haut()
        {
            return StandHaut();
        }

        public override List<Vector2> Bas()
        {
            return StandBas();
        }

        public override List<Vector2> Droite()
        {
            return StandDroite();
        }

        public override List<Vector2> HautDroite()
        {
            return StandDroite();
        }

        public override List<Vector2> BasDroite()
        {
            return StandDroite();
        }

        // ATTAQUER

        // METTRE 7 POUR ARCHER
        public override List<Vector2> AttaquerHaut()
        {
            return StandHaut();
        }

        public override List<Vector2> AttaquerBas()
        {
            return StandBas();
        }

        public override List<Vector2> AttaquerDroite()
        {
            return StandDroite();
        }

        // MORT

        public override List<Vector2> Mort(Unite unite)
        {
            return new List<Vector2> { new Vector2(5,0) };
        }
    }
}
