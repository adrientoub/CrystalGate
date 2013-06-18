using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class HelmetPurple : Stuff
    {

        public HelmetPurple(Unite unite, Vector2 position)
            : base(unite, position)
        {
            Icone = PackTexture.boutons[10];
            type = Type.Casque;
            VieBonus = 50;
            DommagesBonus = 0;
            ManaBonus = 50;
            ArmureBonus = 1;
            ManaRegenBonus = 10;
            PuissanceBonus = 10;
            VitesseBonus = 0;
            VieMaxBonus = VieBonus;
            ManaMaxBonus = ManaBonus;
            id = 7;
        }
    }
}
