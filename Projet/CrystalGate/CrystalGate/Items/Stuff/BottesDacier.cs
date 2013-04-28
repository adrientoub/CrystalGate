using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class BottesDacier : Stuff
    {

        public BottesDacier(Unite unite, Vector2 position)
            : base(unite, position)
        {
            Icone = PackTexture.boutons[7];
            type = Type.Bottes;
            VieBonus = 50;
            DommagesBonus = 50;
            ManaBonus = 50;
            ArmureBonus = 50;
            ManaRegenBonus = 50;
            PuissanceBonus = 50;
            VitesseBonus = 0;
        }
    }
}
