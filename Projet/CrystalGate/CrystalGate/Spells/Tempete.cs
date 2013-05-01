using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    class Tempete : Spell
    {
        const int Portée = 50; // Portée de l'explosion
        const float ratio = 0.05f;
        float rayon = 3;
        Text description1, description2;
        List<Vector2> Positions = new List<Vector2> { };
        Random rand = new Random();

        public Tempete(Unite u, Unite cible, bool useMana = true)
            : base(u, cible)
        {
            Cooldown = 2;
            Ticks = 400;
            CoutMana = 10;
            Animation = PackAnimation.Explosion();
            AnimationReset = PackAnimation.Explosion();
            Tiles = new Vector2(320 / 5, 320 / 5);

            NeedUnPoint = false;
            SpriteBouton = PackTexture.boutons[13];
            SpriteEffect = PackTexture.sorts[0];
            sonSort = new EffetSonore(PackSon.Explosion);

            description1 = new Text("DescriptionTempete1");
            description2 = new Text("DescriptionTempete2");
        }

        public override void UpdateSort()
        {
            Positions.Clear();
            if (TickCurrent == 0)
            {
                rayon = 1;
                Point = ConvertUnits.ToDisplayUnits(unite.body.Position);
            }
            rayon += 0.1f;
            // On ajoute les explosions
            for (double i = 0; i < 2 * Math.PI; i += 0.25f)
                Positions.Add(Point / 32 + new Vector2((float)Math.Cos(i), (float)Math.Sin(i)) * rayon);
            // On verifie les degats sur les unités de la map
            foreach (Vector2 v in Positions)
            {
                foreach (Unite u in Map.unites)
                {
                    float distance = Outil.DistancePoints(v, u.PositionTile);
                    if (u != unite && distance <= Portée)
                    {
                        u.Vie -= (int)(unite.Puissance * ratio - u.DefenseMagique);
                        //u.color = Color.Red;
                    }
                    /*else
                        u.color = Color.White;*/
                }
            }
            //sonSort.Play();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach(Vector2 v in Positions)
                spriteBatch.Draw(SpriteEffect, v * Map.TailleTiles, SpritePosition, Color.White, 0f, new Vector2(Tiles.X / 2, Tiles.Y / 2), 1f, SpriteEffects.None, 0);
        }

        public override string DescriptionSpell()
        {
            return description1.get() + unite.Puissance * ratio * 60 + " " + description2.get();
        }
    }
}
