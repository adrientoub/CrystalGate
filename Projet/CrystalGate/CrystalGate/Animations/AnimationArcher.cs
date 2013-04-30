using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate.Animations
{
    class AnimationArcher : PackAnimation
    {
        public override List<Vector2> AttaquerHaut()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 5; j <= 6; j++)
                liste.Add(new Vector2(0, j));

            return liste;
        }

        public override List<Vector2> AttaquerBas()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 5; j <= 6; j++)
                liste.Add(new Vector2(4, j));

            return liste;
        }

        public override List<Vector2> AttaquerDroite()
        {
            List<Vector2> liste = new List<Vector2> { };
            for (int j = 5; j <= 6; j++)
                liste.Add(new Vector2(2, j));

            return liste;
        }
    }
}
