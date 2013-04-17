using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    class FurieSanguinaire : Spell
    {
        const float ratio = 0.2f;
        Text description;

        public FurieSanguinaire(Unite u, bool useMana = true)
            : base(u)
        {
            Cooldown = 5;
            Ticks = 300;
            if (useMana)
                CoutMana = 30;
            else
                CoutMana = 0;
            Tiles = new Vector2(320 / 5, 320 / 5);

            NeedUnPoint = false;
            SpriteBouton = unite.packTexture.boutons[5];
            SpriteEffect = unite.packTexture.sorts[0];
            sonSort = new EffetSonore(11);
            description = new Text("DescriptionFurieSanguinaire");
        }

        public override void Update()
        {
            if (TickCurrent < Ticks)
            {
                if (TickCurrent == 0)
                {
                    unite.Vitesse_Attaque -= ratio;
                    unite.Scale = 1.25f;
                    unite.color = Color.LightPink;
                }
                else if (TickCurrent == Ticks - 1)
                {
                    unite.Vitesse_Attaque += ratio;
                    unite.Scale = 1f;
                    unite.color = Color.White;
                }
                TickCurrent++;
            }
        }

        public override string DescriptionSpell()
        {
            return description.get() + " " + ratio * 100 + "%.";
        }
    }
}
