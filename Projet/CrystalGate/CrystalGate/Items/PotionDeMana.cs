using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class PotionDeMana : Item
    {
        public PotionDeMana(Vector2 position, PackTexture pack)
            : base(position, pack)
        {
            Icone = pack.boutons[4];
        }

        public override void Effet(Unite unite)
        {
            spell = new RegenMana(unite, false);
        }
    }
}
