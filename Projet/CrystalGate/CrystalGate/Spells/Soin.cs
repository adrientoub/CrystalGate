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
            AnimationLimite = 8;
            Animation = PackAnimation.Explosion();
            Tiles = new Vector2(34, 35);

            NeedUnPoint = false;
            SpriteBouton = unite.packTexture.boutons[1];
            SpriteEffect = unite.packTexture.sorts[1];
            sonSort = new EffetSonore(4);
        }

        public override void Update(Vector2 Point)
        {
            this.Point = Point;
            Animer();
            if(unite.Vie + 1 <= unite.VieMax)
                unite.Vie += 1;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Sprite bouton
            spriteBatch.Draw(SpriteEffect, unite.PositionTile * unite.Map.TailleTiles, SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, SpriteEffects.None, 0);
        }
    }
}
