using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    class RegenMana : Spell
    {
        public RegenMana(Unite u, bool useMana = true)
            : base(u)  
        {
            Cooldown = 2;
            Ticks = 1;
            if (useMana)
                CoutMana = 500;
            else
                CoutMana = 0;
            Animation = PackAnimation.Soin();
            AnimationReset = PackAnimation.Soin();
            Tiles = new Vector2(180 / 5, 35);

            NeedUnPoint = false;
            SpriteBouton = PackTexture.boutons[2];
            SpriteEffect = PackTexture.sorts[2];
            sonSort = new EffetSonore(PackSon.Soin);
        }

        public override void Update()
        {
            Animer();
            if (TickCurrent < Ticks)
            {
                if (unite.Mana != unite.ManaMax)
                {
                    int ammount = unite.ManaMax / 10;
                    if (unite.Mana + ammount <= unite.ManaMax)
                    {
                        unite.Mana += ammount;
                        TickCurrent++;
                    }
                    else
                        unite.Vie = unite.ManaMax;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteEffect, unite.PositionTile * Map.TailleTiles, SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, SpriteEffects.None, 0);
        }
    }
}
