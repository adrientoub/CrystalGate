using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrystalGate
{
    class NovaDeFeu : Spell
    {
        const int Portée = 50; // Portée de l'explosion
        const float ratio = 0.15f;
        float rayon = 3;
        Text description1, description2;
        public List<Vector2> Positions = new List<Vector2> { };
        Random rand = new Random();

        public NovaDeFeu(Unite u, bool useMana = true)
            : base(u)
        {
            idSort = 45;
            Cooldown = 6;
            Ticks = 1200;
            CoutMana = 0;
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
            if (TickCurrent == 0)
            {
                rayon = 0.1f;
                Point = ConvertUnits.ToDisplayUnits(unite.body.Position);
            }
            rayon += 0.0001f;
            // On ajoute les explosions
            if (TickCurrent > 0 && TickCurrent % 3 == 0)
                Positions.Add(Point / 32);
            // Update de la position
            for (int i = 0; i < Positions.Count; i++)
                Positions[i] += new Vector2((float)Math.Cos(i + 1) * rayon, (float)Math.Sin(i + 1) * rayon);

            if (TickCurrent == Ticks - 1)
            {
                rayon = 0.1f;
                Point = ConvertUnits.ToDisplayUnits(unite.body.Position);
            }
            // On verifie les degats sur les unités de la map
            foreach (Vector2 v in Positions)
            {
                foreach (Unite u in Map.unites)
                {
                    float distance = Outil.DistancePoints(v, u.PositionTile);
                    if (u != unite && distance <= Portée)
                    {
                        u.Vie -= (int)(unite.Puissance * ratio  + 1 - u.DefenseMagique);
                        u.color = Color.Red;
                    }
                    else
                        u.color = Color.White;
                }
            }
            sonSort.Play();
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
