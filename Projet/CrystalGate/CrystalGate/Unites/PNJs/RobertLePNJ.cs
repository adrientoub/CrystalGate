using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate.Unites
{
    class Syndra : Voleur
    {
        public Syndra(Vector2 Position)
            : base(Position)
        {
            Vie = 550000;
            isApnj = true;
            FlipH = true;
            direction = Direction.Gauche;
            id = -1;
        }
    }
}
