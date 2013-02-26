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

        public PotionDeVie(Vector2 position, PackTexture pack) : base(position, pack)
        {
            Position = position;
            Icone = pack.boutons[3];
        }

        public override void Effet(Unite unite)
        {
            spell = new Soin(unite);
        }
    }
}
