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

        public Polymorphe(Unite u, Unite cible, bool useMana = true)
            : base(u)
        {
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
            sonSort = new EffetSonore(13);
            description = new Text("DescriptionPolymorph");
        }

        public override void Update()
        {
            if (TickCurrent < Ticks)
            {
                UniteCible.packAnimation = new AnimationCritters();
                UniteCible.Sprite = PackTexture.Critters;
                UniteCible.Tiles = new Vector2(225 / 6, 177 / 4);
                UniteCible.effetUniteDeath = new EffetSonore(14);
                UniteCible.CanAttack = false;
                TickCurrent++;
            }
        }

        public override string DescriptionSpell()
        {
            return description.get();
        }
    }
}
