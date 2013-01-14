using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    class Explosion : Spell
    {
        public Explosion(Unite u)
            : base(u)  
        {
            Cooldown = 1;
            Animation = PackAnimation.Explosion();
            Tiles = new Vector2(65, 65);

            NeedUnPoint = true;
            SpriteBouton = unite.packTexture.boutons[0];
            SpriteEffect = unite.packTexture.sorts[0];
        }

        public override void Update(Vector2 Point)
        {
            this.Point = Point;
            Animer();
            foreach (Unite u in unite.Map.unites)
            {
                float distance = Outil.DistancePoints(this.Point, u.PositionTile);
                if (u != unite && distance <= 75)
                    u.Vie -= 10;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Sprite bouton
            spriteBatch.Draw(SpriteEffect, Point * unite.Map.TailleTiles, SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, SpriteEffects.None, 0);
        }
    }
}
