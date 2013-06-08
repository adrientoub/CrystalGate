using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CrystalGate
{
            [Serializable]
    public class Spell
    {
        public string Nom;
        public Texture2D SpriteBouton; // Sprite du bouton dans la barre des sorts
        public Texture2D SpriteEffect; // Sprite de l'effet du sort
        public Unite unite; // unite qui possède le sort
        public float Cooldown; // temps de recharge
        public int CoutMana;
        public int Ticks;// Le nombre de fois ou la méthode update sera appelé, (ex: le nombre de ticks pour un DOT), 1 par défaut pour un sort instantané
        protected int TickCurrent; // Tick actuel
        public bool NeedUnPoint; // détermine si le sort a besoin d'un point
        public bool NeedAUnit;
        public Vector2 Point; // Point de cible (facultatif) pour le sort
        public Unite UniteCible; // Unite a cibler (facultatif) pour le sort
        public bool Activated; // Determine si le sort doit etre dessiné
        public bool FinDuDrawAtteint; // Determine si on a draw toute l'animation du sort
        public List<Vector2> Animation;// Animation du sort
        public List<Vector2> AnimationReset; // Animation du sort (sert à reset l'animation)
        protected int AnimationCurrent;
        protected int AnimationLimite = 2;
        protected Rectangle SpritePosition; // Position a draw dans le sprite
        public Color Color = Color.White;
        public Vector2 Tiles; // Longueur et hauteur de la tile à Draw

        public float LastCast;

        public EffetSonore sonSort;

        public Spell(Unite u, Unite cible, bool useMana = true)
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
            if (Animation != null)
            {
                if (AnimationCurrent >= AnimationLimite)
                {
                    if (Animation.Count == 0)
                    {
                        FinDuDrawAtteint = true;
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
        }

        public virtual void Update()
        {
            Animer();
            if (TickCurrent < Ticks)
            {
                UpdateSort();
                TickCurrent++;
            }
            else
                if (FinDuDrawAtteint) // Si le sort est fini et a fini d'etre dessiné, on désactive le sort et on remet FinduDraw a false pour une prochaine utilisation
                {
                    FinDuDrawAtteint = false;
                    Activated = false;
                }
        }

        public virtual void UpdateSort()
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void Begin(Vector2 p, Unite unit)
        {
            LastCast = (float)Map.gametime.TotalGameTime.TotalMilliseconds;
            Activated = true;
            TickCurrent = 0;
            sonSort.Play();
            unite.Mana -= CoutMana;
            Point = p;
            UniteCible = unit;
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
