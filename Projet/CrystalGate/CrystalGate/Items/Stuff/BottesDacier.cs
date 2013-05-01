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
            VieBonus = 0;
            DommagesBonus = 0;
            ManaBonus = 0;
            ArmureBonus = 0;
            ManaRegenBonus = 0;
            PuissanceBonus = 0;
            VitesseBonus = 0.05f;
        }
    }
}
