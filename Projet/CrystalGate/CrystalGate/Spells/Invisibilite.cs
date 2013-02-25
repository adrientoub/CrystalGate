using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    class Invisibilite : Spell
    {
        List<Unite> uniteFollowing;

        public Invisibilite(Unite u)
            : base(u)
        {
            Cooldown = 2;
            Ticks = 240;
            CoutMana = 50;
            Tiles = new Vector2(320 / 5, 320 / 5);

            NeedUnPoint = false;
            SpriteBouton = unite.packTexture.boutons[0];
            SpriteEffect = unite.packTexture.sorts[0];
            sonSort = new EffetSonore(5);
            uniteFollowing = new List<Unite>();
        }

        public override void Update()
        {
            if (TickCurrent < Ticks)
            {
                if (TickCurrent == 0)
                {
                    foreach (Unite u in unite.Map.unites)
                    {
                        if (u != unite && u.uniteAttacked == unite)
                        {
                            u.uniteAttacked = null;
                            uniteFollowing.Add(u);
                        }
                    }
                    unite.color = new Color(255, 255, 255, 0);
                }
                else if (TickCurrent == Ticks - 1)
                {
                    unite.color = Color.White;
                    foreach (Unite u in uniteFollowing)
                    {
                        u.uniteAttacked = unite;
                    }
                    uniteFollowing = new List<Unite>();
                }
                TickCurrent++;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteEffect, Point * unite.Map.TailleTiles, SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, SpriteEffects.None, 0);
        }
    }
}
