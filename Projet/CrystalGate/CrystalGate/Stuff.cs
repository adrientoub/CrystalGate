using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Stuff : Item
    {
        public int VieBonus;
        public int DommagesBonus;
        public int ManaBonus;
        public int ArmureBonus;
        public int ManaRegenBonus;
        public int PuissanceBonus;
        public float VitesseBonus;

        public Stuff(Vector2 position, PackTexture pack)
            : base(position, pack)
        {
            Icone = pack.boutons[0];
        }

        public void Equiper()
        {
            unite.Stuff.Add(this);
            unite.Inventory.Remove(this);

            unite.Vie += VieBonus;
            unite.VieMax += VieBonus;
            unite.Dommages += DommagesBonus;
            unite.Puissance += PuissanceBonus;
            unite.Vitesse -= VitesseBonus;
            unite.Defense += ArmureBonus;
            unite.DefenseMagique += ArmureBonus;
            unite.ManaRegen -= ManaRegenBonus;
            unite.Puissance += PuissanceBonus;
        }

        public void Desequiper()
        {
            unite.Inventory.Add(this);
            unite.Stuff.Remove(this);

            unite.Vie -= VieBonus;
            unite.VieMax -= VieBonus;
            unite.Dommages -= DommagesBonus;
            unite.Puissance -= PuissanceBonus;
            unite.Vitesse += VitesseBonus;
            unite.Defense -= ArmureBonus;
            unite.DefenseMagique -= ArmureBonus;
            unite.ManaRegen += ManaRegenBonus;
            unite.Puissance -= PuissanceBonus;
        }
    }
}
