﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    class Explosion : Spell
    {
        const int Portée = 75;
        const float ratio = 2.5f;

        public Explosion(Unite u)
            : base(u)  
        {
            Cooldown = 2;
            Ticks = 1;
            CoutMana = 10;
            Animation = unite.packAnimation.Explosion();
            AnimationReset = unite.packAnimation.Explosion();
            Tiles = new Vector2(320 / 5, 320 / 5);

            NeedUnPoint = true;
            SpriteBouton = unite.packTexture.boutons[0];
            SpriteEffect = unite.packTexture.sorts[0];
            sonSort = new EffetSonore(5);
        }

        public override void Update()
        {
            Animer();
            if (TickCurrent < Ticks)
            {
                foreach (Unite u in unite.Map.unites)
                {
                    float distance = Outil.DistancePoints(this.Point, u.PositionTile);
                    if (u != unite && distance <= Portée)
                    {
                        u.Vie -= (int)(unite.Puissance * ratio - u.DefenseMagique);
                        //u.color = Color.Red;
                    }
                }
                TickCurrent++;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteEffect, Point * unite.Map.TailleTiles, SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, SpriteEffects.None, 0);
        }

        public override string DescriptionSpell()
        {
            return "Creer une explosion dans un rayon de " + Portée + " pixels, infligeant   " + unite.Puissance * ratio + " points de degats magiques.";
        }
    }
}
