using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
    public class Spell
    {
        public string Nom { get; set; }
        public Texture2D SpriteBouton { get; set; }
        public Texture2D SpriteEffect { get; set; }
        public Unite unite { get; set; }
        public float Cooldown { get; set; }
        public bool NeedUnPoint { get; set; }
        public Vector2 Point { get; set; }
        public bool ToDraw { get; set; }

        public List<Vector2> Animation { get; set; }
        protected int AnimationCurrent { get; set; }
        protected int AnimationLimite = 4;
        protected Rectangle SpritePosition { get; set; }
        public Vector2 Tiles { get; set; }

        public float LastCast { get; set; }

        public Spell(Unite u)
        {
            unite = u;
            Cooldown = 1;
            Animation = PackAnimation.Explosion();
            Tiles = new Vector2(65, 65);
        }

        public void Animer()
        {
            if (AnimationCurrent >= AnimationLimite)
            {
                if (Animation.Count == 0)
                {
                    ToDraw = false;
                    Animation = PackAnimation.Explosion();
                }
                else
                {
                    AnimationCurrent = 0;
                    SpritePosition = new Rectangle((int)Animation[0].X * (int)Tiles.X, (int)Animation[0].Y * (int)Tiles.Y, (int)Tiles.X, (int)Tiles.Y);
                    Animation.RemoveAt(0);
                }
            }

            else
                AnimationCurrent++;
        }

        public virtual void Update(Vector2 Point)
        {
            Animer();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
