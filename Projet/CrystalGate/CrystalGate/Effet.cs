using System;
using System.Collections.Generic;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace CrystalGate
{
    public class Effet
    {
        Texture2D Sprite { get; set; }
        Vector2 Position { get; set; }
        Vector2 Tile { get; set; }

        int Taille { get; set; }
        List<Vector2> AnimationCurrent { get; set; }
        List<Vector2> AnimationOriginal { get; set; }
        Rectangle SpritePosition { get; set; }

        int Duree { get; set; }

        int Current { get; set; }

        public Effet(Texture2D Sprite, Vector2 Position, List<Vector2> Animation, Vector2 Tile, int Taille)
        {
            this.Sprite = Sprite;
            this.Position = Position;
            this.Tile = Tile;
            this.Taille = Taille;
            this.AnimationOriginal = this.AnimationCurrent = Animation;

            Duree = 3;

        }

        public void Update()
        {
            
            if (Current >= Duree)
            {
                if (AnimationCurrent.Count > 1)
                    AnimationCurrent.RemoveAt(0);
                else
                    AnimationCurrent = AnimationOriginal;
                Current = 0;
            }
            else
                Current++;

        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            SpritePosition = new Rectangle((int)AnimationCurrent[0].X * (int)Tile.X, (int)AnimationCurrent[0].Y * (int)Tile.Y, (int)Tile.X, (int)Tile.Y);

            spritebatch.Draw(Sprite, Position, SpritePosition, Color.White, 0, new Vector2(Tile.X / 2, Tile.Y / 2), Taille, SpriteEffects.None, 0);
        }
    }
}
