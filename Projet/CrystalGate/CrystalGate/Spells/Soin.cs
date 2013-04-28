using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    class Soin : Spell
    {
        const float ratio = 1.25f;
        Text description1, description2;

        public Soin(Unite u, bool useMana = true)
            : base(u)  
        {
            Cooldown = 2;
            Ticks = 1;
            if (useMana)
                CoutMana = 15;
            else
                CoutMana = 0;
            Animation = PackAnimation.Soin();
            AnimationReset = PackAnimation.Soin();
            Tiles = new Vector2(180 / 5, 35);

            NeedUnPoint = false;
            SpriteBouton = PackTexture.boutons[1];
            SpriteEffect = PackTexture.sorts[1];
            sonSort = new EffetSonore(4);

            description1 = new Text("DescriptionSoin1");
            description2 = new Text("DescriptionSoin2");
        }

        public override void Update()
        {
            Animer();
            if (TickCurrent < Ticks)
            {
                if (unite.Vie != unite.VieMax)
                {
                    int ammount = (int)(unite.Puissance * ratio);
                    if (unite.Vie + ammount <= unite.VieMax)
                    {
                        unite.Vie += ammount;
                        TickCurrent++;
                    }
                    else
                        unite.Vie = unite.VieMax;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteEffect, unite.PositionTile * Map.TailleTiles, SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, SpriteEffects.None, 0);
        }

        public override string DescriptionSpell()
        {
            return description1 + " " + (int)(unite.Puissance * ratio) + " " + description2;
        }
    }
}
