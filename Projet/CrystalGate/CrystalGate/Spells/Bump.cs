using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    class Bump : Spell
    {
        public Bump(Unite u)
            : base(u)
            
        {
            SpriteBouton = unite.packTexture.sorts[0];
        }

        public override void Update()
        {
            foreach (Unite u in unite.Map.unites)
                if(u != unite && Outil.DistanceUnites(unite, u) <= 100)
                    u.body.LinearVelocity = new Vector2((float)Math.Cos(Outil.AngleUnites(u, unite)), (float)Math.Sin(Outil.AngleUnites(u, unite))) * 10;
        }
    }
}
