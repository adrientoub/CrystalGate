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
        public Soin(Unite u, bool useMana = true)
            : base(u)  
        {
            Cooldown = 2;
            Ticks = 1;
            if (useMana)
                CoutMana = 15;
            else
                CoutMana = 0;
            Animation = unite.packAnimation.Soin();
            AnimationReset = unite.packAnimation.Soin();
            Tiles = new Vector2(180 / 5, 35);

            NeedUnPoint = false;
            SpriteBouton = unite.packTexture.boutons[1];
            SpriteEffect = unite.packTexture.sorts[1];
            sonSort = new EffetSonore(4);
        }

        public override void Update()
        {
            Animer();
            if (TickCurrent < Ticks)
            {
                if (unite.Vie != unite.VieMax)
                {
                    int ammount = unite.VieMax / 10;
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
            spriteBatch.Draw(SpriteEffect, unite.PositionTile * unite.Map.TailleTiles, SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, SpriteEffects.None, 0);
        }
    }
}
