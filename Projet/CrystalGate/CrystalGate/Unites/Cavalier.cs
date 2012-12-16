using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Cavalier : Unite
    {

        public Cavalier(Vector2 Position, Map map, SpriteBatch spriteBatch, PackTexture packTexture)
            : base(Position, map, spriteBatch, packTexture)
        {
            // Graphique
            Sprite = packTexture.unites[0];
            Tiles = new Vector2(370 / 5, 835 / 11);

            // Statistique
            Vie = VieMax = 200;
            Vitesse = 3.0f;
            Portee = 2f; // 1 = Corps à corps
            Dommages = 10;
        }


    }
}
