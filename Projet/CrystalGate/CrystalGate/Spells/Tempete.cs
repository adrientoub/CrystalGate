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
        const int Portée = 75; // Portée de l'explosion
        const float ratio = 2.5f;
        const int rayon = 3;
        Text description1, description2, description3;
        List<Vector2> Positions = new List<Vector2> { };
        Random rand = new Random();

        public Tempete(Unite u, Unite cible, bool useMana = true)
            : base(u, cible)
        {
            Cooldown = 2;
            Ticks = 500;
            CoutMana = 10;
            Animation = PackAnimation.Explosion();
            AnimationReset = PackAnimation.Explosion();
            Tiles = new Vector2(320 / 5, 320 / 5);

            NeedUnPoint = false;
            SpriteBouton = PackTexture.boutons[0];
            SpriteEffect = PackTexture.sorts[0];
            sonSort = new EffetSonore(PackSon.Explosion);

            description1 = new Text("DescriptionExplosion1");
            description2 = new Text("DescriptionExplosion2");
            description3 = new Text("DescriptionExplosion3");
        }

        public override void UpdateSort()
        {
            Positions.Clear();

            // On ajoute les explosions
            for (double i = 0; i < 2 * Math.PI; i += 0.2f)
                Positions.Add(ConvertUnits.ToDisplayUnits(unite.body.Position) / 32 + new Vector2((float)Math.Cos(i), (float)Math.Sin(i)) * rayon);
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
            return description1.get() + " " + Portée + " " + description2.get() + " " + unite.Puissance * ratio + " " + description3.get();
        }
    }
}
