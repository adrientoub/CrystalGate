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
        public List<Vector2> AnimationReset { get; set; }
        protected int AnimationCurrent { get; set; }
        protected int AnimationLimite = 4;
        protected Rectangle SpritePosition { get; set; }
        public Vector2 Tiles { get; set; }

        public float LastCast { get; set; }

        public EffetSonore sonSort;

        public Spell(Unite u)
        {
            unite = u;
            AnimationCurrent = AnimationLimite;

            Cooldown = 1;
            Animation = null;
            AnimationReset = null;
            Tiles = Vector2.Zero;

            NeedUnPoint = false;
            SpriteBouton = unite.packTexture.blank;
            SpriteEffect = unite.packTexture.blank;
            sonSort = null;
        }

        public void Animer()
        {
            if (AnimationCurrent >= AnimationLimite)
            {
                if (Animation.Count == 0)
                {
                    ToDraw = false;
                    foreach (Vector2 v in AnimationReset)
                        Animation.Add(v);
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
