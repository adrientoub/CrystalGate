using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Epaulieres : Stuff
    {

        public Epaulieres(Unite unite, Vector2 position)
            : base(unite, position)
        {
            Icone = PackTexture.boutons[8];
            type = Type.Epaulieres;
            VieBonus = 0;
            DommagesBonus = 0;
            ManaBonus = 0;
            ArmureBonus = 5;
            ManaRegenBonus = 0;
            PuissanceBonus = 0;
            VitesseBonus = 0;
        }
    }
}
