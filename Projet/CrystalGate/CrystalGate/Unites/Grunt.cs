using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CrystalGate
{
    public class Grunt : Unite
    {

        public Grunt(Vector2 Position, Map map, SpriteBatch spriteBatch, PackTexture packTexture)
            : base(Position, map, spriteBatch, packTexture)
        {
            // Graphique
            Sprite = packTexture.unites[1];
            Tiles = new Vector2( 380 / 5, 600 / 11);

            // Statistiques
            Vie = VieMax = 100;
            Vitesse = 3.0f;
            Portee = 1; // 1 = Corps à corps
            Dommages = 1;
        }


    }
}
