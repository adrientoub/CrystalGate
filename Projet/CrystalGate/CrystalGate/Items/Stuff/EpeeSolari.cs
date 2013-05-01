using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class EpeeSolari : Stuff
    {

        public EpeeSolari(Unite unite, Vector2 position)
            : base(unite, position)
        {
            Icone = PackTexture.boutons[6];
            type = Type.Arme;
            VieBonus = 100;
            DommagesBonus = 25;
            ManaBonus = 50;
            ArmureBonus = 0;
            ManaRegenBonus = 0;
            PuissanceBonus = 25;
            VitesseBonus = 0;
        }
    }
}
