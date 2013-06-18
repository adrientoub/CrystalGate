using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class PotionDeVie : Item
    {

        public PotionDeVie(Unite unite, Vector2 position) : base(unite, position)
        {
            Icone = PackTexture.boutons[3];
            spell = new Soin(unite, false);
            id = 1;
        }

        public override void Effet(Unite unite)
        {
            spell = new Soin(unite, false);
        }
    }
}
