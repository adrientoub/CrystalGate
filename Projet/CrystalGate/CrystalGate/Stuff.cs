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
        public int VieMaxBonus;
        public int DommagesBonus;
        public int ManaBonus;
        public int ManaMaxBonus;
        public int ArmureBonus;
        public int ManaRegenBonus;
        public int PuissanceBonus;
        public float VitesseBonus;

        public Stuff(Unite unite, Vector2 position)
            : base(unite, position)
        {
            Icone = PackTexture.boutons[0];
        }

        public void Equiper()
        {
            if (unite.Stuff.Where(i => i.type == this.type).ToList().Count == 0) // Si on est pas deja equipé de ce type de pièce
            {
                unite.Stuff.Add(this);
                unite.Inventory.Remove(this);

                unite.VieMaxBonus += VieMaxBonus;

                unite.ManaMaxBonus += ManaMaxBonus;
                unite.DommagesBonus += DommagesBonus;
                unite.PuissanceBonus += PuissanceBonus;
                unite.VitesseBonus += VitesseBonus;
                unite.DefenseBonus += ArmureBonus;
                unite.DefenseMagiqueBonus += ArmureBonus;
                unite.ManaRegenBonus -= ManaRegenBonus;
                unite.PuissanceBonus += PuissanceBonus;
            }
        }

        public void Desequiper()
        {
                unite.Inventory.Add(this);
                unite.Stuff.Remove(this);


                unite.VieMaxBonus -= VieMaxBonus;

                unite.ManaMaxBonus -= ManaMaxBonus;
                unite.DommagesBonus -= DommagesBonus;
                unite.PuissanceBonus -= PuissanceBonus;
                unite.VitesseBonus -= VitesseBonus;
                unite.DefenseBonus -= ArmureBonus;
                unite.DefenseMagiqueBonus -= ArmureBonus;
                unite.ManaRegenBonus += ManaRegenBonus;
                unite.PuissanceBonus -= PuissanceBonus;
        }
    }
}
