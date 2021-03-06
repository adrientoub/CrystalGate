﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    class Bump : Spell
    {
        public Bump(Unite u, bool useMana = true)
            : base(u)
            
        {
            Cooldown = 0;
            SpriteBouton = PackTexture.sorts[0];
        }

        public override void UpdateSort()
        {
            foreach (Unite u in Map.unites)
                if (u != unite && Outil.DistanceUnites(unite, u) <= 500)
                {
                    u.uniteAttacked = null;
                    u.ObjectifListe.Clear();
                    u.body.LinearVelocity = new Vector2((float)Math.Cos(Outil.AngleUnites(u, unite)), (float)Math.Sin(Outil.AngleUnites(u, unite))) * 100;
                }
        }
    }
}
