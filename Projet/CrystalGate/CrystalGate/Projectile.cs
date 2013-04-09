using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Projectile
    {
        Texture2D Sprite;
        Unite Tireur;
        Unite Target;
        int Vitesse;
        Vector2 Position;
        public int Timer;

        public Projectile(Unite tireur, Unite target)
        {
            Sprite = tireur.ProjectileSprite;
            Position = ConvertUnits.ToDisplayUnits(tireur.body.Position);
            Tireur = tireur;
            Target = target;
            Vitesse = 10;
            Timer = (int)Outil.DistanceUnites(tireur, target) / Vitesse;
        }

        public void Update()
        {
            Position += new Vector2((float)Math.Cos(Outil.AngleUnites(Target, Tireur)), (float)-Math.Sin(Outil.AngleUnites(Tireur, Target))) * Vitesse;
            Timer--;
        }

        public bool IsInWall()
        {
            // Toutes les sprites dont l'Y est <= 4 sont des murs (cf la sprite des tiles)
            return Tireur.Map.Cellules[(int)(Position.X / Tireur.Map.TailleTiles.X), (int)(Position.Y / Tireur.Map.TailleTiles.Y)].Y <= 4;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, null, Color.White, Outil.AngleUnites(Target, Tireur), new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 0);
        }
    }
}
