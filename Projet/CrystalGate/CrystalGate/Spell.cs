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
        public Texture2D SpriteBouton { get; set; } // Sprite du bouton dans la barre des sorts
        public Texture2D SpriteEffect { get; set; } // Sprite de l'effet du sort
        public Unite unite { get; set; } // unite qui possède le sort
        public float Cooldown { get; set; } // temps de recharge
        public int CoutMana { get; set; }
        public int Ticks { get; set; } // Le nombre de fois ou la méthode update sera appelé, (ex: le nombre de ticks pour un DOT), 1 par défaut pour un sort instantané
        protected int TickCurrent; // Tick actuel
        public bool NeedUnPoint { get; set; } // détermine si le sort a besoin d'un point
        public Vector2 Point { get; set; } // Point de cible (facultatif) pour le sort
        public bool ToDraw { get; set; } // Determine si le sort doit etre dessiné

        public List<Vector2> Animation { get; set; } // Animation du sort
        public List<Vector2> AnimationReset { get; set; } // Animation du sort (sert à reset l'animation)
        protected int AnimationCurrent { get; set; }
        protected int AnimationLimite = 2;
        protected Rectangle SpritePosition { get; set; } // Position a draw dans le sprite
        public Color Color = Color.White;
        public Vector2 Tiles { get; set; } // Longueur et hauteur de la til à Draw

        public float LastCast { get; set; }

        public EffetSonore sonSort;

        public Spell(Unite u)
        {
            unite = u;
            AnimationCurrent = AnimationLimite;

            Cooldown = 1;
            Ticks = 1;
            CoutMana = 1;
            Animation = null;
            AnimationReset = null;
            Tiles = Vector2.Zero;

            NeedUnPoint = false;
            SpriteBouton = PackTexture.blank;
            SpriteEffect = PackTexture.blank;
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

        public virtual void Update()
        {
            Animer();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Begin(Vector2 p)
        {
            LastCast = (float)Map.gametime.TotalGameTime.TotalMilliseconds;
            ToDraw = true;
            TickCurrent = 0;
            sonSort.Play();
            unite.Mana -= CoutMana;
            Point = p;
        }

        public virtual string DescriptionSpell()
        {
            return "null";
        }

        public override string ToString()
        {
            return base.ToString().Split(new char[1] { '.' })[1];;
        }
    }
}
