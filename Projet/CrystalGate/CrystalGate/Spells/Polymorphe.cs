using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CrystalGate.Animations;

namespace CrystalGate
{
    class Polymorphe : Spell
    {
        const float ratio = 0.2f;
        Text description;

        public Polymorphe(Unite u, bool useMana = true)
            : base(u)
        {
            idSort = 4;
            Cooldown = 2;
            Ticks = 1;
            if (useMana)
                CoutMana = 30;
            else
                CoutMana = 0;

            NeedUnPoint = false;
            NeedAUnit = true;
            SpriteBouton = PackTexture.boutons[12];
            SpriteEffect = PackTexture.sorts[0];
            sonSort = new EffetSonore(PackSon.PolymorphCible);
            description = new Text("DescriptionPolymorph");
        }

        public override void UpdateSort()
        {
            if (!UniteCible.isAChamp)
            {
                UniteCible.packAnimation = new AnimationCritters();
                UniteCible.Sprite = PackTexture.Critters;
                UniteCible.Tiles = new Vector2(225 / 6, 177 / 4);
                UniteCible.effetUniteDeath = new EffetSonore(PackSon.SheepDeath);
                UniteCible.CanAttack = false;
            }
        }

        public override string DescriptionSpell()
        {
            return description.get();
        }
    }
}
