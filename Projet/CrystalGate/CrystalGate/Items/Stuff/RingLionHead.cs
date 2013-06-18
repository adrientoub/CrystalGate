using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class RingLionHead : Stuff
    {

        public RingLionHead(Unite unite, Vector2 position)
            : base(unite, position)
        {
            Icone = PackTexture.boutons[11];
            type = Type.Anneau;
            VieBonus = 50;
            DommagesBonus = 0;
            ManaBonus = 0;
            ArmureBonus = 0;
            ManaRegenBonus = 0;
            PuissanceBonus = 10;
            VitesseBonus = 0;
            VieMaxBonus = VieBonus;
            ManaMaxBonus = ManaBonus;
            id = 8;
        }
    }
}
