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
        public Soin(Unite u)
            : base(u)  
        {
            Cooldown = 1;
            Ticks = 1;
            Animation = unite.packAnimation.Soin();
            AnimationReset = unite.packAnimation.Soin();
            Tiles = new Vector2(180 / 5, 35);

            NeedUnPoint = false;
            SpriteBouton = unite.packTexture.boutons[1];
            SpriteEffect = unite.packTexture.sorts[1];
            sonSort = new EffetSonore(4);
        }

        public override void Update(Vector2 Point)
        {
            Animer();
            this.Point = Point;
            if (TickCurrent < Ticks)
            {
                if (unite.Vie != unite.VieMax)
                {
                    int ammount = 10;
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
            // Sprite bouton
            spriteBatch.Draw(SpriteEffect, unite.PositionTile * unite.Map.TailleTiles, SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, SpriteEffects.None, 0);
        }
    }
}
